## HVH.Service
This is a semi-professional project, maintained by the (student-)administrators of the Hermann-von-Helmholtz Gymnasium in Potsdam, Germany. It aims to provides an easy and stable solution for remote-controlling remote computers in an enterprise network. (like a big company, or a school). It is divided into multiple parts. 

HVH.Service provides the execution components of the system. It works by connecting to a central server who is sending commands to the client node. The client node then performs the neccessarry actions, like locking the computer or shutting it down.

### Configuration
The app reads its settings from an ini file, called settings.ini. This is an example config:
```
[connection]
server=192.168.99.1
port=77799

[security]
keySize=1024
encryption=NoneEncryptionProvider
```
The rest is configured dynamically through the server.

### License
HVH.Client as is is licensed under the MIT License. It provides interfaces for loading additional components, who can be licensed differently. The whole project has no global license, unless otherwise specified, the parts are licensed as All Rights Reserved and currently not open sourced.

### Credits
HVH.Client is written by Dorian Stoll (@ThomasKerman)
The whole project is maintained by Dorian Stoll and Kai Münch.
