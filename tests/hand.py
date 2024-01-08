from collections import namedtuple
import cv2
import mediapipe as mp


mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_hands = mp.solutions.hands

# /sync/IMI/env-imi/bin/python
# For webcam input:
cap = cv2.VideoCapture(0)
width  = cap.get(cv2.CAP_PROP_FRAME_WIDTH)   # float `width`
height = cap.get(cv2.CAP_PROP_FRAME_HEIGHT)  # float `height`

Point = namedtuple("Point", "x y")
class Tracking_point:

  t = 0
  p = Point(0,0)

  def __init__(self, x, y):
    self.p = Point(x,y)


l = [Point(0,0) for k in range(10)]


with mp_hands.Hands(
    model_complexity=0,
    max_num_hands=4,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5) as hands:
  while cap.isOpened():
    success, image = cap.read()
    if not success:
      print("Ignoring empty camera frame.")
      # If loading a video, use 'break' instead of 'continue'.
      continue

    # To improve performance, optionally mark the image as not writeable to
    # pass by reference.
    image.flags.writeable = False
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    results = hands.process(image)
    # Draw the hand annotations on the image.
    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
    if results.multi_hand_landmarks:
      for hand_landmarks in results.multi_hand_landmarks:
        mp_drawing.draw_landmarks(
            image,
            hand_landmarks,
            mp_hands.HAND_CONNECTIONS,
            mp_drawing_styles.get_default_hand_landmarks_style(),
            mp_drawing_styles.get_default_hand_connections_style())
        
        l.pop(0)
        l.append(Point(hand_landmarks.landmark[8].x, hand_landmarks.landmark[8].y))

        lm = hand_landmarks.landmark
        print(type(lm[8]))


    
        
      
      cv2.circle(image, (round(width*hand_landmarks.landmark[8].x), round(height*hand_landmarks.landmark[8].y)), 5, 200) 

    for k in range(len(l)):
      p = l[k]
      cv2.circle(image, (round(width*p.x), round(height*p.y)), k+5, 200)
    # Flip the image horizontally for a selfie-view display.
    cv2.imshow('MediaPipe Hands', cv2.flip(image, 1))

    if cv2.waitKey(5) & 0xFF == 27:
      break
cap.release() 