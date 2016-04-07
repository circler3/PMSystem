# PMSystem
This is a C/S group work management project using supersocket. It aims
at group members' working progress management.

There are three components in this project.

1. PMServer

 PMServer is the central server of the system. both previleged client and normal client communicate with the server.
server buffers messages and sends messages to its destination.

2. Previleged client

 Previleged client is plays a role of administrator of the system. It is able to recieve others' status message from
 the server.
 
3. Normal client

 Normal client will report its working status to the server by the schedule. And it receives the command from the
 previleged client. 

 