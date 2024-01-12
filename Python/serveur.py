import numpy as np
import socket
import json
import time
import cv2

################################################################################
# configuration
################################################################################

# Serveur
UDP_IP = "127.0.0.1"
UDP_PORT = 5065


sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

################################################################################
# Loop
################################################################################

start = time.time()
while True :
    print(f"run '{time.time()-start}")

    message = json.dumps({
        'C': time.time()-start, 
        })
    sock.sendto( message.encode(), (UDP_IP, UDP_PORT) )

    key = cv2.waitKey(1)
    if key == ord('q') or key == 27:
        break

