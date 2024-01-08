import mediapipe as mp
import cv2



class Hands:
    def __init__(self) -> None:
        self.mpHands = mp.solutions.hands
        self.hands = self.mpHands.Hands()
        self.mpDraw = mp.solutions.drawing_utils
    
    def process(self,image):
        self.results = self.hands.process(image)

    def draw(self,image,skelet = False):
        if self.results.multi_hand_landmarks:
            for handLms in self.results.multi_hand_landmarks: # working with each hand
                for id, lm in enumerate(handLms.landmark):
                    h, w, c = image.shape
                    cx, cy = int(lm.x * w), int(lm.y * h)
                    cv2.circle(image, (cx, cy), 5, (255, 0, 255), cv2.FILLED)
                if skelet:
                    self.mpDraw.draw_landmarks(image, handLms, self.mpHands.HAND_CONNECTIONS)
        return image

