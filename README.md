# Secure WebApi Server (C#)
The Server of SecureWebApi.</br>
This is project contains a secure web api auth library and sample. </br>
You can modify it to suit your business logic.</br>
Server language is C#.Using .Net Framework 4.6.1,And IIS host it.</br>

>Attention Please:</br>
"SecureWebApiServer/ConsoleApp/WebApi/AppKeySecret.cs" is a key/secret generator.</br>
You can give it to your client which will invoke your api,please remember secret don't tranfer on Internet. Client and Server shoudl keep it safely.</br>


Now,You have a secure web api.
Server will verify every request before perform it.If the request is wrong ,server will return unauthorized msg to client.

##Screen Shot
IIS status:
![IIS](https://github.com/wind0ws/SecureWebApiServer/blob/master/screenshot/IIS.png)

