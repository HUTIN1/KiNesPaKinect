import socket
import json

class Connection:
    def __init__(self,upd_ip,upd_port) -> None:
        self.UDP_IP = upd_ip
        self.UDP_PORT = upd_port
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)


    def __call__(self,messages : dict) -> any:

        message = json.dumps(messages)
        self.sock.sendto( message.encode(), (self.UDP_IP, self.UDP_PORT) )
