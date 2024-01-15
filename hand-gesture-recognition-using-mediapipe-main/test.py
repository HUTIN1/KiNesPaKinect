import numpy as np
import cv2 as cv
cap = cv.VideoCapture('video_data/two_hands4.avi')
nb_frame = 0
while cap.isOpened():
    ret, frame = cap.read()
    # if frame is read correctly ret is True
    if not ret:
        print("Can't receive frame (stream end?). Exiting ...")
        break
    nb_frame += 1
    #gray = cv.cvtColor(frame, cv.COLOR_BGR2GRAY)
    cv.imshow('frame', frame)
    if cv.waitKey(1) == ord('q'):
        break
print(nb_frame)
cap.release()
cv.destroyAllWindows()