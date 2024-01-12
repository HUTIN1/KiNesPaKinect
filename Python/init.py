import argparse
import cv2 as cv
import mediapipe as mp
import csv
import os

import Connection
import keypoint_classifier

class Initialiszation:
    def __init__(self) -> None:
        self.args = self.get_args()
        self.init_path = os.path.dirname(os.path.realpath(__file__))
        self.path_classifier_one_hand = os.path.join(self.init_path,"label","keypoint_classifier_label.csv")
        self.path_model_one_hand = os.path.join(self.init_path,"model","keypoint_classifier.tflite")
        self.UDP_IP = "127.0.0.1"
        self.UDP_PORT = 5065


    def __call__(self):
        cap_device = self.args.device
        cap_width = self.args.width
        cap_height = self.args.height

        use_static_image_mode = self.args.use_static_image_mode
        min_detection_confidence = self.args.min_detection_confidence
        min_tracking_confidence = self.args.min_tracking_confidence

        cap = self._camera(cap_device,cap_height=cap_height, cap_width=cap_width)

        hands = self._mediapide(
            use_static_image_mode=use_static_image_mode,
            min_detection_confidence=min_detection_confidence,
            min_tracking_confidence=min_tracking_confidence)

        keypoint_classifier_labels = self._classifier_labels_one_hand()

        serveur = self._serveur()

        one_hand_classifier = self._nn_one_hand()


        return cap, hands, keypoint_classifier_labels, serveur, one_hand_classifier


    def _serveur(self):
        serveur = Connection.Connection(self.UDP_IP,self.UDP_PORT)
        return serveur

    def _nn_one_hand(self):      
        return keypoint_classifier.KeyPointClassifier(model_path=self.path_model_one_hand)
    
    def _camera(self,cap_device,cap_height,cap_width):
        # カメラ準備 ###############################################################
        cap = cv.VideoCapture(cap_device)
        cap.set(cv.CAP_PROP_FRAME_WIDTH, cap_width)
        cap.set(cv.CAP_PROP_FRAME_HEIGHT, cap_height)

        return cap
    
    def _mediapide(self,use_static_image_mode,min_detection_confidence,min_tracking_confidence):
        mp_hands = mp.solutions.hands
        hands = mp_hands.Hands(
        static_image_mode=use_static_image_mode,
        max_num_hands=1,
        min_detection_confidence=min_detection_confidence,
        min_tracking_confidence=min_tracking_confidence,
        )
        return hands
    
    def _classifier_labels_one_hand(self):
        with open(self.path_classifier_one_hand,
            encoding='utf-8-sig') as f:
            keypoint_classifier_labels = csv.reader(f)
            keypoint_classifier_labels = [
            row[0] for row in keypoint_classifier_labels
            ]
        return keypoint_classifier_labels

    def get_args(self):
        parser = argparse.ArgumentParser()

        parser.add_argument("--device", type=int, default=0)
        parser.add_argument("--width", help='cap width', type=int, default=960)
        parser.add_argument("--height", help='cap height', type=int, default=540)

        parser.add_argument('--use_static_image_mode', action='store_true')
        parser.add_argument("--min_detection_confidence",
                            help='min_detection_confidence',
                            type=float,
                            default=0.7)
        parser.add_argument("--min_tracking_confidence",
                            help='min_tracking_confidence',
                            type=int,
                            default=0.5)

        args = parser.parse_args()

        return args