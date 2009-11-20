/*
 * Synchronization primitives.
 * See synch.h for specifications of the functions.
 */

#include <types.h>
#include <lib.h>
#include <synch.h>
#include <thread.h>
#include <curthread.h>
#include <machine/spl.h>

////////////////////////////////////////////////////////////
//
// Semaphore.

struct semaphore *
sem_create(const char *namearg, int initial_count)
{
	struct semaphore *sem;

	sem = kmalloc(sizeof(struct semaphore));
	if (sem == NULL) {
		return NULL;
	}

	sem->name = kstrdup(namearg);
	if (sem->name == NULL) {
		kfree(sem);
		return NULL;
	}

	sem->count = initial_count;
	return sem;
}

void
sem_destroy(struct semaphore *sem)
{
	int spl;
	assert(sem != NULL);

	spl = splhigh();
	assert(thread_hassleepers(sem)==0);
	splx(spl);

	/*
	 * Note: while someone could theoretically start sleeping on
	 * the semaphore after the above test but before we free it,
	 * if they're going to do that, they can just as easily wait
	 * a bit and start sleeping on the semaphore after it's been
	 * freed. Consequently, there's not a whole lot of point in 
	 * including the kfrees in the splhigh block, so we don't.
	 */

	kfree(sem->name);
	kfree(sem);
}

void 
P(struct semaphore *sem)
{
	int spl;
	assert(sem != NULL);

	/*
	 * May not block in an interrupt handler.
	 *
	 * For robustness, always check, even if we can actually
	 * complete the P without blocking.
	 */
	assert(in_interrupt==0);

	spl = splhigh();
	while (sem->count==0) {
		thread_sleep(sem);
	}
	assert(sem->count>0);
	sem->count--;
	splx(spl);
}

void
V(struct semaphore *sem)
{
	int spl;
	assert(sem != NULL);
	spl = splhigh();
	sem->count++;
	assert(sem->count>0);
	thread_wakeup(sem);
	splx(spl);
}

////////////////////////////////////////////////////////////
//
// Lock.

struct lock *
lock_create(const char *name)
{
	struct lock *lock;

	lock = kmalloc(sizeof(struct lock));
	if (lock == NULL) {
		return NULL;
	}

	lock->name = kstrdup(name);
	if (lock->name == NULL) {
		kfree(lock);
		return NULL;
	}
	
	// BEGIN SOL1
	lock->holder = NULL;
	// END SOL1
	
	return lock;
}

void
lock_destroy(struct lock *lock)
{
	assert(lock != NULL);

	// BEGIN SOL1
	// nothing else required here
	// END SOL1
	
	kfree(lock->name);
	kfree(lock);
}

void
lock_acquire(struct lock *lock)
{
	// BEGIN SOL1
	int spl;

	assert(lock != NULL);

	// Turn off interrupts so we can work atomically. 
	spl = splhigh();

	// If we already hold the lock, it's bad.
	if (lock_do_i_hold(lock)) {
		panic("lock_acquire: lock %s at %p: Deadlock.\n",
		      lock->name, lock);
	}

	// Wait for it to become free.
	while (lock->holder != NULL) {
		thread_sleep(lock);
	}

	// Now acquire it for ourselves.
	lock->holder = curthread;

	// Return interrupts to previous level.
	splx(spl);

	// END SOL1
}

void
lock_release(struct lock *lock)
{
	// BEGIN SOL1
	int spl;

	assert(lock != NULL);

	// Turn off interrupts.
	spl = splhigh();

	// Must hold the lock to be allowed to release it.
	if (lock->holder != curthread) {
		panic("lock_release: lock %s at %p: Not holder.\n",
		      lock->name, lock);
	}

	// Release the lock.
	lock->holder = NULL;

	/*
	 * By waking up everyone on the lock, we allow the scheduler
	 * to make priority-based decisions about who runs next
	 * (avoiding priority inversion).  This is worth the overhead
	 * of all threads who don't acquire the lock waking up and
	 * immediately going back to sleep.
	 */
	thread_wakeup(lock);
	
	// Return interrupts to previous level.
	splx(spl);

	// END SOL1
}

int
lock_do_i_hold(struct lock *lock)
{
	// BEGIN SOL1
	int spl, ret;

	assert(lock != NULL);

	// Turn off interrupts.
	spl = splhigh();

	ret = (lock->holder == curthread);

	// Return interrupts to previous level.
	splx(spl);

	return ret;
	// END SOL1
}

////////////////////////////////////////////////////////////
//
// CV


struct cv *
cv_create(const char *name)
{
	struct cv *cv;

	cv = kmalloc(sizeof(struct cv));
	if (cv == NULL) {
		return NULL;
	}

	cv->name = kstrdup(name);
	if (cv->name==NULL) {
		kfree(cv);
		return NULL;
	}
	
	// BEGIN SOL1
	// nothing additional required
	// END SOL1
	
	return cv;
}

void
cv_destroy(struct cv *cv)
{
	assert(cv != NULL);

	// BEGIN SOL1
	// nothing additional required
	// END SOL1
	
	kfree(cv->name);
	kfree(cv);
}

void
cv_wait(struct cv *cv, struct lock *lock)
{
	// BEGIN SOL1
	int spl;

	assert(cv != NULL);
	assert(lock != NULL);
	
	// Turn off interrupts.
	spl = splhigh();

	// Let go of the lock.
	lock_release(lock);

	// Sleep until someone signals or broadcasts on the CV.
	thread_sleep(cv);
	
	// Get the lock back.
	lock_acquire(lock);
	
	// Return interrupts to previous level.
	splx(spl);

	// END SOL1
}

void
cv_signal(struct cv *cv, struct lock *lock)
{
	// BEGIN SOL1
	int spl;

	assert(cv != NULL);
	assert(lock != NULL);

	// Turn off interrupts.
	spl = splhigh();

	// Must hold the lock to signal.
	if (!lock_do_i_hold(lock)) {
		panic("cv_signal: cv %s at %p, lock %s at %p: Not holder.\n",
		      cv->name, cv, lock->name, lock);
	}

	// Just wake one thread up.
	thread_wakeone(cv);

	// Return interrupts to previous level.
	splx(spl);

	// END SOL1
}

void
cv_broadcast(struct cv *cv, struct lock *lock)
{
	// BEGIN SOL1
	int spl;

	assert(cv != NULL);
	assert(lock != NULL);

	// Turn off interrupts.
	spl = splhigh();

	// Must hold the lock to broadcast.
	if (!lock_do_i_hold(lock)) {
		panic("cv_broadcast: cv %s at %p, lock %s at %p: "
		      "Not holder.\n",
		      cv->name, cv, lock->name, lock);
	}

	// Wake 'em all.
	thread_wakeup(cv);

	// Return interrupts to previous level.
	splx(spl);

	// END SOL1
}
