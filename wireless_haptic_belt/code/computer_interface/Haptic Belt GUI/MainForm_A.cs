using System;
using System.Data;
using System.Threading;
using HapticDriver;

/* MainForm_A - Main Form Activation functions */

namespace HapticGUI
{
    partial class GUI
    {    
        TimeSpan fiftyMS = new TimeSpan(50 * TimeSpan.TicksPerMillisecond);
        TimeSpan timer;

        DateTime startTime;
        DateTime currentTime;

        // Activates selected motor from AddedList
        // Note that Abort() must be called on the thread running this function to break out of activation (Stop Button)
        private void Activate_Activation()
        {
            //We don't care about performance of a single motor so just use the String version of Vibrate_Motors()
            Activation selected = _group[_current_group].events[_current_event].activations[_current_activation];

            if (selected.motor < _motorcount) //if selected motor is currently connected try to activate it
            {
                //Note if cycles = 7 (infinity), this timer won't be used
                timer = new TimeSpan(_group[_current_group].rhythm[selected.rhythm].time * 50 * selected.cycles * TimeSpan.TicksPerMillisecond);

                //Vibrate Motor
                belt.Vibrate_Motor(selected.motor, selected.rhythm, selected.magnitude, selected.cycles);

                if (selected.cycles != 7)
                {
                    startTime = DateTime.UtcNow;
                    currentTime = DateTime.UtcNow; // Get current time

                    //Wait for the motor to finish
                    while (timer > currentTime.Subtract(startTime))
                    {
                        Thread.Sleep(sleepTime);
                        currentTime = DateTime.UtcNow;
                    }
                }
                else
                    while (true) //Wait for stop to be issued
                        Thread.Sleep(sleepTime);
            }
        }
        // Activates currently selected event now, ignoring all events before or after this event.
        // Note that Abort() must be called on the thread running this function to break out of activation (Stop Button)
        private void Activate_Event()
        {
            int i, eventEndTime = 0, endTime = 0;
            Activation selected;
            
            //Calculate the eventEndTime, the time at which all motors in the event will finish
            for (i = 0; i < _group[_current_group].events[_current_event].activations.Length; i++)
            {
                selected = _group[_current_group].events[_current_event].activations[i];
                if(selected.cycles != 7)
                {
                    endTime = _group[_current_group].rhythm[selected.rhythm].time * 50 * selected.cycles;
                    if(eventEndTime < endTime)
                        eventEndTime = endTime;
                }
                else //set eventEndTime to infinity, nothing is greater than infinity so break.
                {
                    eventEndTime = -1;
                    break;
                }
            }

            //Activate all connected motors in this event
            for (i = 0; i < _group[_current_group].events[_current_event].activations.Length; i++)
            {
                selected = _group[_current_group].events[_current_event].activations[i];

                if (selected.motor < _motorcount) //if selected motor is currently connected try to activate it
                {
                    //Vibrate Motor
                    belt.Vibrate_Motor(selected.motor, selected.rhythm, selected.magnitude, selected.cycles);
                }
            }

            //if the event completes in a finite time
            if (eventEndTime != -1)
            {
                startTime = DateTime.UtcNow;
                currentTime = DateTime.UtcNow; // Get current time

                timer = new TimeSpan(eventEndTime * TimeSpan.TicksPerMillisecond);

                //Wait for the motor to finish
                while (timer > currentTime.Subtract(startTime))
                {
                    Thread.Sleep(sleepTime);
                    currentTime = DateTime.UtcNow;
                }
            }
            else
                while (true) //Wait for stop to be issued
                    Thread.Sleep(sleepTime);
        }

        // Activates selected group from GroupList, all motors in sed group are activated based
        // on the parameters entered by the user upon adding that motor to the group.
        // Motors that are not viewable are ignored.
        // Note that Abort() must be called on the thread running this function to break out of activation (Stop Button)
        private void Activate_Group()
        {
            int h, i, j, groupEndTime = 0;

            //Find the longest activation time on the last event
            //If a single last activation on a particular motor is Infinity (cycles = 7), the endGroupTime will
            //be infinity.
            for (i = 0; i < _motorcount; i++) //up to 16 iterations
            {
                if (_group[_current_group].motors[i].endTime == -1)
                {
                    groupEndTime = -1;
                    break;
                }
                else if (groupEndTime < _group[_current_group].motors[i].endTime)
                    groupEndTime = _group[_current_group].motors[i].endTime;
            }

            if (_group[_current_group].cycles != 0) //Limited Cycles
            {
                for (h = 0; (h < _group[_current_group].cycles); h++)
                {
                    startTime = DateTime.UtcNow;
                    for (i = 0; i < _group[_current_group].events.Length; i++)
                    {
                        // Grab the event time, and create a TimeSpan for that event time
                        timer = new TimeSpan(_group[_current_group].events[i].time * TimeSpan.TicksPerMillisecond);
                        currentTime = DateTime.UtcNow; // Get current time

                        // Wait until the next event occurs, or stop button is hit
                        while (timer > currentTime.Subtract(startTime))
                        {
                            /* responseTime has a value of 0 to 99ms
                             * 1 : after 1ms the thread will be inserted into the queue to not bog down the CPU as much
                             * Worse case is we are roughly 1ms late breaking out of this loop, shouldn't be a concern.
                             * 
                             * 0 : Reset thread to top of thread queue, then execute again.
                             * This is an attempt to not hog ALL resources even in high Resolution mode.
                             * Because we only have a 10ms resolution with DateTime.UtcNow anyways.
                             */
                            Thread.Sleep(responseTime);

                            /* DateTime.UtcNow reads a register directly, thus has an extremely fast 
                             * excess time of around 100ns. Resolution times as as follows:
                             *      Windows NT 3.5 and later 10 milliseconds
                             *      Windows 98 55 milliseconds
                             *      
                             * Thus this program is not Windows 98 friendly, we operating in 50ms chunks.
                             */
                            currentTime = DateTime.UtcNow;
                        }
                        //Vibrate all motors present on the belt
                        for (j = 0; j < _group[_current_group].events[i].activations.Length; j++)
                        {
                            if(_group[_current_group].events[i].activations[j].motor < _motorcount) //if motor is present
                                belt.Vibrate_Motor(_group[_current_group].events[i].activations[j].motor, _group[_current_group].events[i].activations[j].rhythm, _group[_current_group].events[i].activations[j].magnitude, _group[_current_group].events[i].activations[j].cycles);
                        }
                    }
                    //Wait for the last event to finish, unless its infinte
                    if (groupEndTime != -1)
                    {
                        timer = new TimeSpan(groupEndTime * TimeSpan.TicksPerMillisecond);
                        currentTime = DateTime.UtcNow; // Get current time

                        while (timer > currentTime.Subtract(startTime))
                        {
                            Thread.Sleep(responseTime);
                            currentTime = DateTime.UtcNow;
                        }
                    }
                    else
                        while (true)
                            Thread.Sleep(sleepTime);
                }
            }
            else //Unlimited Cycles
            {
                while(true)
                {
                    startTime = DateTime.UtcNow;
                    for (i = 0; i < _group[_current_group].events.Length; i++)
                    {
                        // Grab the event time, and create a TimeSpan for that event time
                        timer = new TimeSpan(_group[_current_group].events[i].time * TimeSpan.TicksPerMillisecond);
                        currentTime = DateTime.UtcNow; // Get current time

                        // Wait until the next event occurs, or stop button is hit
                        while (timer > currentTime.Subtract(startTime))
                        {
                            /* responseTime has a value of 0 to 99ms
                             * 1 : after 1ms the thread will be inserted into the queue to not bog down the CPU as much
                             * Worse case is we are roughly 1ms late breaking out of this loop, shouldn't be a concern.
                             * 
                             * 0 : Reset thread to top of thread queue, then execute again.
                             * This is an attempt to not hog ALL resources even in high Resolution mode.
                             * Because we only have a 10ms resolution with DateTime.UtcNow anyways.
                             */
                            Thread.Sleep(responseTime);

                            /* DateTime.UtcNow reads a register directly, thus has an extremely fast 
                             * excess time of around 100ns. Resolution times as as follows:
                             *      Windows NT 3.5 and later 10 milliseconds
                             *      Windows 98 55 milliseconds
                             *      
                             * Thus this program is not Windows 98 friendly, we operating in 50ms chunks.
                             */
                            currentTime = DateTime.UtcNow;
                        }
                        //Vibrate all motors present on the belt
                        for (j = 0; j < _group[_current_group].events[i].activations.Length; j++)
                        {
                            if (_group[_current_group].events[i].activations[j].motor < _motorcount) //if motor is present
                                belt.Vibrate_Motor(_group[_current_group].events[i].activations[j].motor, _group[_current_group].events[i].activations[j].rhythm, _group[_current_group].events[i].activations[j].magnitude, _group[_current_group].events[i].activations[j].cycles);
                        }   
                    }
                    //Wait for the last event to finish, unless its infinte
                    if (groupEndTime != -1)
                    {
                        timer = new TimeSpan(groupEndTime * TimeSpan.TicksPerMillisecond);
                        currentTime = DateTime.UtcNow; // Get current time

                        while (timer > currentTime.Subtract(startTime))
                        {
                            Thread.Sleep(responseTime);
                            currentTime = DateTime.UtcNow;
                        }
                    }
                    else
                        while (true)
                            Thread.Sleep(sleepTime); //Timing is not important as the user cant hit Stop then Activate within 200ms
                }
            }
        }

        private void Stop_Activations()
        { 
            activate_trd.Abort(); //stop the activations
            activate_trd.Join(); //wait for activate_trd thread to rejoin

            //Stop All Motors, user may see up to a 200ms delay in response this should be an acceptable tradeoff of performance for less CPU utilization
            if (hasError(belt.StopAll(), "StopAll()"))
            {
                //Handle Error
            } 
        }
    }
}