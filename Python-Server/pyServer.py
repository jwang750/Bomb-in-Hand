import socket
#from Server import *


class PyServer:
    def __init__(self,PORT):
        HOST = ""
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.s.bind((HOST, PORT))
        self.s.listen(1)
        print("waiting...",PORT)
        self.conn, addr = self.s.accept()
        print("connected!")

    def __del__(self):
        self.conn.close()
        self.s.close()

    def update_k(self):
        msg = "action accumulate "
        self.conn.sendall(bytes(msg, encoding="utf-8"))

    def update_l(self):
        msg = "action throw "
        self.conn.sendall(bytes(msg, encoding="utf-8"))

    def update_n(self):
        msg = "action no "
        self.conn.sendall(bytes(msg, encoding="utf-8"))

    def update_m(self,final):
        msg = "action"
        for i in range(len(final)):
            msg = msg + " " + final[i]
        print(msg)
        self.conn.sendall(bytes(msg, encoding="utf-8"))

    def waiting(self):
        data = self.conn.recv(1024)
        msg = data.decode()
        s = msg.split(" ")
        #print(s)
        return s
