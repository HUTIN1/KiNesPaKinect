from collections import deque
from collections import Counter
from typing import Any
import time

class Combo:
    def __init__(self,two_hands_keypoint_classifier_labels,history_length=16) -> None:
        self.hands_sign_history = deque(maxlen=history_length)
        self.two_hands_keypoint_classifier_labels = two_hands_keypoint_classifier_labels
        self.starter_label = "Hajime"
        self.Combo = {"Katone" : ["Cheval","Illuminati","Jul"]}
        self._combo_tmp = {"Katone":0}
        self.mode_game = 0
        self.timer_combo = 0
        self.previous_hands_sign = 0

    def __call__(self, two_hands_sign_id) -> Any:
        combo = {}
        self.hands_sign_history.append(two_hands_sign_id)
        most_common_hd_id = Counter(self.hands_sign_history).most_common()
        if most_common_hd_id[0][0] != 0 and most_common_hd_id[0][0] != self.previous_hands_sign:
    
            self.previous_hands_sign = most_common_hd_id[0][0]
            print(f"2 hands {self.two_hands_keypoint_classifier_labels[self.previous_hands_sign]}")
            
            if self.mode_game == 0 and self.two_hands_keypoint_classifier_labels[self.previous_hands_sign] == self.starter_label :
                print("Debut du combo")
                self.mode_game = 1
                self.timer_combo = time.time() + 5

            if self.mode_game == 1 :
                combo = self.detectCombo(self.previous_hands_sign)

                
            
        if self.mode_game == 1 and self.timer_combo - time.time()  <= 0:
            print("end combo")
            self.mode_game = 0


        return combo  
    

    def detectCombo(self,two_hands_sign_id):
        result = {key : False for key in self.Combo.keys()}
        for key in self.Combo.keys() :
            if self.Combo[key][self._combo_tmp[key]] == self.two_hands_keypoint_classifier_labels[two_hands_sign_id]:
                self._combo_tmp[key] +=1
            else :
                self._combo_tmp[key] = 0

            if self._combo_tmp[key] == len(self.Combo[key]):
                result[key] = True
                self._combo_tmp[key] = 0
                print(f"{key}")
                        
        return result
    

    def isInCombo(self):
        return self.mode_game == 1


            
