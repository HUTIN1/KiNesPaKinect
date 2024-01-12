#!/usr/bin/env python
# -*- coding: utf-8 -*-


import cv2 as cv

import utils
import init


def main():
    # 引数解析 #################################################################
    Init = init.Initialiszation()
    (cap , 
     hands, 
     keypoint_classifier_labels,
     serveur,
     keypoint_classifier) = Init()
    

    while True:

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

                # ランドマークの計算
                landmark_list = utils.calc_landmark_list(image, hand_landmarks) #normalize par rapport à la taille de l'image

                # 相対座標・正規化座標への変換
                pre_processed_landmark_list = utils.pre_process_landmark(
                    landmark_list) #centre les landmark par rapport à un point de référence

                # ハンドサイン分類
                hand_sign_id = keypoint_classifier(pre_processed_landmark_list) #classfier des landmark
                print(f"sign : {keypoint_classifier_labels[hand_sign_id]}, hand sign id : {hand_sign_id}")

                message = {'C': float(hand_sign_id)}
                serveur(message)


        # 画面反映 #############################################################

if __name__ == '__main__':
    main()
