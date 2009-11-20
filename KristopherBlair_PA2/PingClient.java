import java.io.*;
import java.net.*;
import java.util.*;

/*
 * Server to process ping requests over UDP.
 */
public class PingClient
{
   private static final double LOSS_RATE = 0.3;
   private static final int AVERAGE_DELAY = 100;  // milliseconds

   public static void main(String[] args) throws Exception
   {
      long time = 0;
      long accumTime = 0;
      int lostPackets = 0;
      byte[] message = new byte[1024];
      int port;
      InetAddress address;
      // Get command line argument.
      if (args.length != 2) {
         System.out.println("Required arguments not met defaulting {host, port} to: Local Host, 6113");
         port = 6113;
         address = InetAddress.getByName("127.0.0.1");
      }
      else
      {
		 address = InetAddress.getByName(args[0]);
     	 port = Integer.parseInt(args[1]);
	  }
      // Create random number generator for use in simulating
      // packet loss and network delay.
      Random random = new Random();

      // Create a datagram socket for receiving and sending UDP packets
      // through the port and host specified on the command line.
      DatagramSocket socket = new DatagramSocket(port, address);

      //Wait approximately 1 second for a response (1000 miliseconds)
      socket.setSoTimeout(1000);

	  for(int i = 0; i<10; i++)
	  {
		  // Create a datagram packet to hold incomming UDP packet.
		  DatagramPacket response = new DatagramPacket(new byte[1024], 1024);

		  //Create a byte encoded message
		  message = ("PING " + Integer.toString(i) + " " + Calendar.getInstance().getTime() + "\n\r").getBytes();

		  //Create a DatagramPacket to send DatagramPacket(byte[],offset,length)
		  DatagramPacket packet = new DatagramPacket(message, message.length, address , 6112);

		  //Send the packet
		  socket.send(packet);

		  //Get the current time
		  time = System.currentTimeMillis();

		  try
		  {
			 // Block until the server responds with a UDP packet.
			 socket.receive(response);

			 // Print the recieved data.
			 printData(response);

			 //Grab Elapsed Time
			 time = System.currentTimeMillis() - time;

			 //Print RTT
			 System.out.println(time + "ms");

			 //Accumulate time to later do average
			 accumTime += time;
		  }
		  //Assumes packet was lost, increment count of packets lost
		  catch(SocketTimeoutException e)
		  {
			 //Accumulate number of lost packets
			 lostPackets++;
		  }
	  }
	  //Print Lost Packets
	  System.out.println("\nNumber Of Lost Packets: " + lostPackets);

	  //Calculate average RTT of Packets, and print it
	  System.out.println("Average RTT Of Packets: " + accumTime/(10-lostPackets));
   }

   /*
    * Print ping data to the standard output stream.
    */
   private static void printData(DatagramPacket request) throws Exception
   {
      // Obtain references to the packet's array of bytes.
      byte[] buf = request.getData();

      // Wrap the bytes in a byte array input stream,
      // so that you can read the data as a stream of bytes.
      ByteArrayInputStream bais = new ByteArrayInputStream(buf);

      // Wrap the byte array output stream in an input stream reader,
      // so you can read the data as a stream of characters.
      InputStreamReader isr = new InputStreamReader(bais);

      // Wrap the input stream reader in a bufferred reader,
      // so you can read the character data a line at a time.
      // (A line is a sequence of chars terminated by any combination of \r and \n.)
      BufferedReader br = new BufferedReader(isr);

      // The message data is contained in a single line, so read this line.
      String line = br.readLine();

      // Print host address and data received from it.
      System.out.println(
         "Received from " +
         request.getAddress().getHostAddress() +
         ": " +
         new String(line) );
   }
}


