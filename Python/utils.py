import copy
import itertools

import numpy as np

def pre_process_landmark(landmark_list):
    temp_landmark_list = copy.deepcopy(landmark_list)

    # 相対座標に変換
    base_x, base_y = 0, 0
    for index, landmark_point in enumerate(temp_landmark_list):
        if index == 0:
            base_x, base_y = landmark_point[0], landmark_point[1]

        temp_landmark_list[index][0] = temp_landmark_list[index][0] - base_x
        temp_landmark_list[index][1] = temp_landmark_list[index][1] - base_y

    # 1次元リストに変換
    temp_landmark_list = list(
        itertools.chain.from_iterable(temp_landmark_list))

    # 正規化
    max_value = max(list(map(abs, temp_landmark_list)))

    def normalize_(n):
        return n / max_value

    temp_landmark_list = list(map(normalize_, temp_landmark_list))

    return temp_landmark_list


def calc_landmark_list(image, landmarks):
    image_width, image_height = image.shape[1], image.shape[0]

    landmark_point = []

    # キーポイント
    for _, landmark in enumerate(landmarks.landmark):
        landmark_x = min(int(landmark.x * image_width), image_width - 1)
        landmark_y = min(int(landmark.y * image_height), image_height - 1)
        # landmark_z = landmark.z

        landmark_point.append([landmark_x, landmark_y])

    return landmark_point



def angle_hand(landmark_list):
    angle = np.arccos((landmark_list[8][0]-landmark_list[0][0]) / np.sqrt((landmark_list[0][0]-landmark_list[8][0])**2 + (landmark_list[0][1]-landmark_list[8][1])**2))
    return np.round(90 - 180*angle/np.pi)

def is2Hands(results):
    outbool = True
    l, r = -1, -1
    if len(results.multi_hand_landmarks)==2:    
        #Detection d'une main gauche et d'une main droite
        if results.multi_handedness[0].classification[0].index == 0:
            l = 0
        elif results.multi_handedness[1].classification[0].index == 0:
            l = 1

        if results.multi_handedness[0].classification[0].index == 1:
            r = 0
        elif results.multi_handedness[1].classification[0].index == 1:
            r = 1


    if r == l or l == -1 or r == -1:
        outbool = False
        
    return outbool, r, l


def normalize_two_landmark(landmarks_0, landmarks_1):
    
    #Calcul distance base paume/bout majeur
    d = landmark_distance(landmarks_0.landmark, 0, 9) + landmark_distance(landmarks_0.landmark, 9, 10) + landmark_distance(landmarks_0.landmark, 10, 11) + landmark_distance(landmarks_0.landmark, 11, 12)
    base_x, base_y = 0, 0
    for index, landmark in enumerate(landmarks_0.landmark):
        if index == 0:
            base_x = landmark.x
            base_y = landmark.y
        landmark.x = (landmark.x-base_x)/d
        landmark.y = (landmark.y-base_y)/d
        #landmark.z = 

    for _, landmark in enumerate(landmarks_1.landmark):

        landmark.x = (landmark.x-base_x)/d
        landmark.y = (landmark.y-base_y)/d
        #landmark.z = 

    return landmarks_0, landmarks_1



def landmark_distance(landmark, ind_1, ind_2):
    return np.sqrt((landmark[ind_1].x
    -landmark[ind_2].x)**2 + (landmark[ind_1].y
    -landmark[ind_2].y)**2 + (landmark[ind_1].z
    -landmark[ind_2].z)**2)



def landmark_to_list(landmarks):
    
    landmark_point = []

    for _, landmark in enumerate(landmarks.landmark):
        landmark_x = landmark.x
        landmark_y = landmark.y
        # landmark_z = landmark.z

        landmark_point.append([landmark_x, landmark_y])

    return landmark_point