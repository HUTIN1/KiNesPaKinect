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
    norme_index = 0
    landmark_katana = [6, 8] #Id landmark pour orientation katana
    Katana_pos, Katana_dir, Katana_garde = [0.0,0.0], [0.0,0.0,0.0], False
    s = False
    
    

    while True:
        jump = False  
        forward = False
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
                    if handedness.classification[0].index == 0: #Main gauche
                        if hand_sign_id == 0: #Calcul de l'angle si main ouverte et avance
                            forward = True
                            angle = utils.angle_hand(landmark_list)
                        else: #Calcul de l'angle si main fermé et ne bouge pas
                            angle = utils.angle_hand(landmark_list)

                    elif handedness.classification[0].index == 1: #Main droite
                        if hand_sign_id != left_hand_hand_sign: #Si on a une nouvelle position
                            left_hand_hand_sign = hand_sign_id
                            jump = False
                            if left_hand_hand_sign == 0: #Main ouverte: Saut
                                jump = True
                        
                        if hand_sign_id == 3:#Si ciseau mise a jour norme
                            norme_index = utils.landmark_distance_2d(hand_landmarks.landmark, landmark_katana[0], landmark_katana[1])

                        if hand_sign_id != -1:#Deplacement katana si main ouverte
                            
                            #P: Position de la main (base majeur) dans l'image
                            #V: Vecteur directeur de la lame du katana
                            Katana_pos, Katana_dir = utils.info_katana(hand_landmarks, norme_index, landmark_katana)
                            if hand_sign_id == 0: #Mode garde si main droite ouverte
                                Katana_dir[2] = 0
                                Katana_dir = utils.normalize(Katana_dir)
                                Katana_garde = True
                            else:
                                Katana_garde = False
                            

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
            s = not s
            message = {'C': float(hand_sign_id), "Jump":jump,"Angle":angle,"Forward":forward,
                       "Kat_pos":(Katana_pos),"Kat_dir":(Katana_dir),"Kat_garde":Katana_garde,
                       "S":s}
            message = {**message,**results_combo}

            print(message)
                
            serveur(message)
        


        # 画面反映 #############################################################

if __name__ == '__main__':
    main()
