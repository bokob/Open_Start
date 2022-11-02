import sys
import os
import numpy as np
import cv2

from PIL import Image
from PIL import ImageGrab
from pytesseract import *
import matplotlib.pyplot as plt
import matplotlib.image as mpimg
import re

# tesseract 파일 다운로드
# 아래 파일은 https://github.com/UB-Mannheim/tesseract/wiki 여기에서 다운로드 하면 됩
pytesseract.tesseract_cmd = R'C:\Program Files\Tesseract-OCR\tesseract'
# 한국어 ocr 학습 데이터는
# https://github.com/tesseract-ocr/tessdata/blob/main/kor.traineddata 에서 다운받아야 아래 코드 실행가능

# 이미 crop된 이미지(혹은 원하는 부위만 찍은 이미지)일 경우 --
# img = cv2.imread("./test_img/test_img/test(3).jpg")

# hsv = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

# conf = '-l kor+eng --oem 3 --psm 4'

# text = pytesseract.image_to_string(img,config=conf)

# -- 여기까지

# 원하는 부분(명패)을 crop하고 싶은 경우

class ocr_detection:
    def __init__(self):
        self.dir_name = ""
        self.file_name = ""
        self.path = ""
    
    def setDirName(self,dir_name): # 입력 이미지의 디렉토리를 넣어줌
        self.dir_name = dir_name
    def setFileName(self,file_name): # crop할 이미지 이름 넣어줌
        self.file_name = file_name
    def setResultPath(self,path):
        self.path = path
    def getResult(self):
        # 입력이미지와 템플릿 이미지 읽기
        img = cv2.imread(self.dir_name+self.file_name+'.jpg') # 입력 이미지
        print(self.dir_name)
        print(self.file_name)
        template = cv2.imread(self.dir_name+'test(12).jpg') # 찾는 대상=명패 부분을 crop하기 위해서 명패와 유사한 부분을 이미지에서 찾도록 함
        th, tw = template.shape[:2]
        # cv2.imshow('template', template) # 그냥 이미지 출력하는 부분이라 뺌

        # 원본 이미지에서 특정 이미지를 찾는 방법을 적용
        # 원본 이미지에 템플릿 이미지(비교 이미지)를 좌측상단 부터 미끄러지듯이 우측으로 이동하면서 계속 비교
        # 3가지 매칭 메서드 순회
        methods = ['cv2.TM_CCOEFF_NORMED', 'cv2.TM_CCORR_NORMED', \
                                            'cv2.TM_SQDIFF_NORMED']
        for i, method_name in enumerate(methods):
            img_draw = img.copy()
            method = eval(method_name)
            # 템플릿 매칭   ---①
            res = cv2.matchTemplate(img, template, method) # 탬플릿(비교이미지) 매칭을 위한 함수
            # 최솟값, 최댓값과 그 좌표 구하기 ---②
            min_val, max_val, min_loc, max_loc = cv2.minMaxLoc(res)
            #print(method_name, min_val, max_val, min_loc, max_loc)

            # TM_SQDIFF의 경우 최솟값이 좋은 매칭, 나머지는 그 반대 ---③
            if method in [cv2.TM_SQDIFF, cv2.TM_SQDIFF_NORMED]:
                top_left = min_loc
                match_val = min_val
            else:
                top_left = max_loc
                match_val = max_val
            # 매칭 좌표 구해서 사각형 표시   ---④      
            bottom_right = (top_left[0] + tw, top_left[1] + th)
            cv2.rectangle(img_draw, top_left, bottom_right, (0,0,255),2) # 매칭 부분을 표시 하기 위해 사각형을 그리는 함수(cv2.rectangle)를 사용
            # 매칭 포인트 표시 ---⑤
            cv2.putText(img_draw, str(match_val), top_left, \
                        cv2.FONT_HERSHEY_PLAIN, 2,(0,255,0), 1, cv2.LINE_AA) # cv2.putText 함수는 이미지에 글자를 넣어주는 함수
            cv2.imshow(method_name, img_draw) # 매칭 부분을 표시한 이미지를 출력해냄
            
            copy_img = img[top_left[1]:bottom_right[1],top_left[0]:bottom_right[0]].copy()
            cv2.imwrite(self.path+self.file_name+'_copy_'+str(i)+'.png',copy_img) # crop한 이미지 파일 저장
        cv2.waitKey(0)
        cv2.destroyAllWindows()
        # crop된 이미지 출력 안하려면, imshow 함수랑 윗 두줄 없애면 됨
        # (현재는 코드 실행 후, 텍스트 결과보기 전에 crop된 이미지 뜸)

        img_list_np = []
        
        path = self.path

        for i in os.listdir(path):
            img3 = cv2.imread(path+i)

            hsv3 = cv2.cvtColor(img3, cv2.COLOR_BGR2RGB)

            conf = ('-l kor+eng --oem 3 --psm 6')

            text3 = pytesseract.image_to_string(hsv3, config=conf)
            print(f'{i} : ' + text3) # text3에 인식한 글자가 저장, 출력