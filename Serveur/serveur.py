import numpy as np
import cv2 as cv
import socket
import json
import time

################################################################################
# configuration
################################################################################

# Paramètres grilles
CHECKERBOARD = (11, 8)
grid_mm = 20

# Paramètres intrinsèques webcam 
sensor_mm = np.array([3.58, 2.685])
# sensor_mm = np.array([3.2, 2.4])
focal_mm = 4 
resolution = np.array([1280, 960])
distortion = np.zeros((4, 1))
center = (resolution[0]/2, resolution[1]/2)

# Webcam
id_cam1 = 4
id_cam2 = 6

# Serveur
UDP_IP = "127.0.0.1"
UDP_PORT = 5065


# Recherche des points
criteria = (cv.TERM_CRITERIA_EPS + cv.TERM_CRITERIA_MAX_ITER, 30, 0.001)

################################################################################
# Initialisation
################################################################################

# Points de la grille
objpoints = np.zeros((CHECKERBOARD[0]*CHECKERBOARD[1], 3), np.float32)
objpoints[:, :2] = np.mgrid[0:grid_mm*CHECKERBOARD[0]:grid_mm, 0:grid_mm*CHECKERBOARD[1]:grid_mm].T.reshape(-1, 2)

# Matrice instinsèque
m_cam = np.array([[focal_mm*resolution[0]/sensor_mm[0], 0, center[0]], 
                [0, focal_mm*resolution[1]/sensor_mm[1], center[1]],
                [0, 0, 1]], dtype="double")

# UDP serveur
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Démarrage des cameras
try:
    cap1 = cv.VideoCapture(id_cam1)
    cap2 = cv.VideoCapture(id_cam2)
    ret = cap1.set(cv.CAP_PROP_FRAME_WIDTH,resolution[0])
    ret = cap1.set(cv.CAP_PROP_FRAME_HEIGHT,resolution[1])
    ret = cap2.set(cv.CAP_PROP_FRAME_WIDTH,resolution[0])
    ret = cap2.set(cv.CAP_PROP_FRAME_HEIGHT,resolution[1])
except:
    print("No Camera Found!")

################################################################################
# Toolbox
################################################################################

# Calibration d'une camera
def calibrate(frame):
    gray = cv.cvtColor(frame, cv.COLOR_BGR2GRAY)
    ret, corners = cv.findChessboardCorners(gray, CHECKERBOARD, None)
    if ret:
        corners2 = cv.cornerSubPix(gray, corners, CHECKERBOARD, (-1, -1), criteria)
        return cv.solvePnP(objpoints, corners2, m_cam, distortion, flags=0)
    return ret, None, None

# Calcul de la matrice de passage de camera 1 à caméra 2
def getM12(r1, t1, r2, t2):
    rm1, _ = cv.Rodrigues(r1)
    rm2, _ = cv.Rodrigues(r2)
    rm12 = np.dot(rm2, rm1.T)
    r12, _ = cv.Rodrigues(rm12) 
    t12 = t2 - np.dot(rm12,t1)
    return r12, t12

# Calcul des paramètres extrinsèques de camera 2 depuis camera 1
def getM2fromM1andM12(r1, t1, r12, t12):
    rm1, _ = cv.Rodrigues(r1)
    rm12, _ = cv.Rodrigues(r12)
    r2, _ = cv.Rodrigues(np.dot(rm12, rm1))
    t2 = np.dot(rm12, t1)+ t12
    return r2, t2

# Affiche les résultats de la calibration
def show(frame, pw, r, t, m, d):
    projected, _ = cv.projectPoints(pw, r, t, m, d)
    cv.drawChessboardCorners(frame, CHECKERBOARD, projected, True)

################################################################################
# Calibration 2 cameras
################################################################################
calibrated = False

while not calibrated and cap1.isOpened() and cap2.isOpened():
    ret1, frame1 = cap1.read()
    ret2, frame2 = cap2.read()

    frame_calc = frame2.copy()

    if ret1 and ret2: 
        ret1, r1, t1 = calibrate(frame1)
        ret2, r2, t2 = calibrate(frame2)

        if ret1 and ret2:
            if not calibrated:
                r12, t12 = getM12(r1, t1, r2, t2)
                r2calc, t2calc = getM2fromM1andM12(r1, t1, r12, t12)
                calibrated = True

                show(frame1, objpoints, r1, t1, m_cam, distortion)
                show(frame2, objpoints, r2, t2, m_cam, distortion)
                show(frame_calc, objpoints, r2calc, t2calc, m_cam, distortion)

        cv.imshow('img1', frame1)
        cv.imshow('img2', frame2)
        cv.imshow('test', frame_calc)
    
    key = cv.waitKey(1)
    if key == ord('q') or key == 27:
        break

# cv.destroyAllWindows()
cap2.release()

################################################################################
# Loop
################################################################################


while cap1.isOpened() :
    ret, frame = cap1.read()
    if ret: 
        ret, r1, t1 = calibrate(frame)
        if ret:
            r2calc, t2calc = getM2fromM1andM12(r1, t1, r12, t12)
            rm, _ = cv.Rodrigues(r2calc)
            message = json.dumps({
                'C': id_cam2,
                'M': m_cam.reshape(-1).tolist(), 
                'R': r2calc.T.tolist()[0],
                'T': t2calc.T.tolist()[0], 
                'F': rm[:,2].tolist(), 
                'U': rm[:,1].T.tolist(), 
                })
            sock.sendto( message.encode(), (UDP_IP, UDP_PORT) )
        # cv.imshow('img1', frame)

    key = cv.waitKey(1)
    if key == ord('q') or key == 27:
        break

cap1.release()

cv.destroyAllWindows()
