#!/usr/bin/env python
# -*- coding: utf-8 -*-


import cv2 as cv

import utils
import init


def main():
    # 引数解析 #################################################################
    Init = init.Initialiszation()
    (cap, 
     hands, 
     keypoint_classifier_labels_one_hand,
     serveur,
     one_hand_keypoint_classifier,
     two_hand_classifier,
     keypoint_classifier_labels_two_hand,
     combo) = Init()
    
    mode_game = 0
    left_hand_hand_sign = -1
    two_hands_hand_sign_id = 0

    
    

    while True:
        Jump = 0 #0 ne saute pas, 1 saute
        angle = 0
        results_combo = {"Katone":False}

        # キー処理(ESC：終了) #################################################
        key = cv.waitKey(10)
        if key == 27:  # ESC
            break

        # カメラキャプチャ #####################################################
        ret, image = cap.read()
        if not ret:
            break
        image = cv.flip(image, 1)
        image = cv.cvtColor(image, cv.COLOR_BGR2RGB)

        image.flags.writeable = False
        results = hands.process(image) #process landmark
        image.flags.writeable = True

        #  ####################################################################
        if results.multi_hand_landmarks is not None:
            for hand_landmarks, handedness in zip(results.multi_hand_landmarks,
                                                  results.multi_handedness):
                
                landmark_list = utils.calc_landmark_list(image, hand_landmarks) #normalize par rapport à la taille de l'image
                pre_processed_landmark_list = utils.pre_process_landmark(
                    landmark_list) #centre les landmark par rapport à un point de référence

                hand_sign_id = one_hand_keypoint_classifier(pre_processed_landmark_list) #classfier des landmark

                if mode_game == 0 :
                    if handedness.classification[0].index == 1: #Main droite
                        if hand_sign_id == 0: #Calcul de l'angle si main ouverte
                            angle = utils.angle_hand(landmark_list)

                    elif handedness.classification[0].index == 0: #Main gauche
                        if hand_sign_id != left_hand_hand_sign: #Si on a une nouvelle position
                            left_hand_hand_sign = hand_sign_id
                            Jump = 0
                            if left_hand_hand_sign == 0: #Main ouverte: Saut
                                Jump = 1

                hands2 , idxright, idxleft = utils.is2Hands(results)
                if hands2 :
                    hand_landmarks_0 = results.multi_hand_landmarks[idxleft]
                    hand_landmarks_1 = results.multi_hand_landmarks[idxright]
                    landmark_list_0 = utils.landmark_to_list(hand_landmarks_0)
                    landmark_list_1 = utils.landmark_to_list(hand_landmarks_1)
                    pre_processed_landmark_list = utils.pre_process_landmark(landmark_list_0+landmark_list_1)
                    
                    hand_landmarks_0, hand_landmarks_1 = utils.normalize_two_landmark(hand_landmarks_0, hand_landmarks_1)
                    two_hands_hand_sign_id = two_hand_classifier(pre_processed_landmark_list)

                    results_combo = combo(two_hands_hand_sign_id)


                if combo.isInCombo():
                    mode_game = 1

                else :
                    mode_game = 0


                
                # print(f"sign : {keypoint_classifier_labels_one_hand[hand_sign_id]}, hand sign id : {hand_sign_id}")

                message = {'C': float(hand_sign_id), "Jump":Jump,"Angle":angle}
                
                serveur({**message,**results_combo})


        # 画面反映 #############################################################

if __name__ == '__main__':
    main()
