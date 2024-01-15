import numpy as np
import cv2 as cv
import csv

labels = [[], []]

with open('model/keypoint_classifier/keypoint_classifier_label.csv',
            encoding='utf-8-sig') as f:
    keypoint_classifier_labels = csv.reader(f)
    labels[0] = [
        row[0] for row in keypoint_classifier_labels
    ]

with open('model/two_hands_keypoint_classifier/two_hands_keypoint_classifier_label.csv',
            encoding='utf-8-sig') as f:
    two_hands_keypoint_classifier_labels = csv.reader(f)
    labels[1] = [
        row[0] for row in two_hands_keypoint_classifier_labels
    ]


cap = cv.VideoCapture(0)
# Define the codec and create VideoWriter object
fourcc = cv.VideoWriter_fourcc(*'DIVX')


mode = 0
number = 0
rec = False #True if we are writing frames
can_rec = False #True if a recorder is set

mode_list = ["one_hand", "two_hands"]



while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        print("Can't receive frame (stream end?). Exiting ...")
        break
    frame = cv.flip(frame, 1)

    # write the frame
    if rec and can_rec:
        out.write(frame)


    cv.putText(frame, mode_list[mode] + f"_{number} ({labels[mode][number]})", (10, 60),
               cv.FONT_HERSHEY_SIMPLEX, 0.6, (0, 0, 0), 4, cv.LINE_AA)
    cv.putText(frame, mode_list[mode] + f"_{number} ({labels[mode][number]})", (10, 60),
               cv.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 2, cv.LINE_AA)
    if rec == True:
        cv.putText(frame, "Recording...", (10, 90),
               cv.FONT_HERSHEY_SIMPLEX, 0.6, (0, 0, 0), 4, cv.LINE_AA)
        cv.putText(frame, "Recording...", (10, 90),
               cv.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 2, cv.LINE_AA)
    elif can_rec == True:
        cv.putText(frame, "Can start record", (10, 90),
               cv.FONT_HERSHEY_SIMPLEX, 0.6, (0, 0, 0), 4, cv.LINE_AA)
        cv.putText(frame, "Can start record", (10, 90),
               cv.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 2, cv.LINE_AA)
    
    cv.imshow('frame', frame)
    key = cv.waitKey(1)
    if key == ord('q'): #Quit
        break
    elif key == ord('s'): #Save
        if not rec:
            out.release()
            can_rec = False
    elif key == ord('r'): #Record
        rec = not rec

    if not can_rec: #Si pas de recorder
        if key == ord('i'):
            out = cv.VideoWriter("video_data/" + mode_list[mode] + f"{number}.avi", fourcc, 20.0, (640,  480), True)
            can_rec = True
        elif 48 <= key <= 57:  # 0 ~ 9
            number = min(key - 48, len(labels[mode])-1)
        elif key == ord('h'):  # Mode une main
            mode = 0
            number = min(number, len(labels[mode])-1)
        elif key == ord('t'):  # Mode deux mains
            mode = 1
            number = min(number, len(labels[mode])-1)

        

# Release everything if job is finished
cap.release()
cv.destroyAllWindows()