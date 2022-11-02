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
# 입력이미지와 템플릿 이미지 읽기
dir_name = './test_img/test_img/' # 입력 이미지의 디렉토리를 넣어줌

img_name = 'test(13)' # crop할 이미지 이름 넣어줌

img = cv2.imread(dir_name+img_name+'.jpg') # 입력 이미지
template = cv2.imread('./test_img/test_img/test(12).jpg') # 찾는 대상=명패 부분을 crop하기 위해서 명패와 유사한 부분을 이미지에서 찾도록 함
th, tw = template.shape[:2]
# cv2.imshow('template', template) # 그냥 이미지 출력하는 부분이라 뺌

# 3가지 매칭 메서드 순회
methods = ['cv2.TM_CCOEFF_NORMED', 'cv2.TM_CCORR_NORMED', \
                                     'cv2.TM_SQDIFF_NORMED']
for i, method_name in enumerate(methods):
    img_draw = img.copy()
    method = eval(method_name)
    # 템플릿 매칭   ---①
    res = cv2.matchTemplate(img, template, method)
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
    cv2.rectangle(img_draw, top_left, bottom_right, (0,0,255),2)
    # 매칭 포인트 표시 ---⑤
    cv2.putText(img_draw, str(match_val), top_left, \
                cv2.FONT_HERSHEY_PLAIN, 2,(0,255,0), 1, cv2.LINE_AA)
    cv2.imshow(method_name, img_draw)
    
    copy_img = img[top_left[1]:bottom_right[1],top_left[0]:bottom_right[0]].copy()
    cv2.imwrite('./result/'+img_name+'_copy_'+str(i)+'.png',copy_img) # crop한 이미지 파일 저장
cv2.waitKey(0)
cv2.destroyAllWindows()

path = './result/'
img_list_np = []

for i in os.listdir(path):
    img3 = cv2.imread(path+i)

    hsv3 = cv2.cvtColor(img3, cv2.COLOR_BGR2RGB)

    conf = ('-l kor+eng --oem 3 --psm 6')

    text3 = pytesseract.image_to_string(hsv3, config=conf)
    print(f'{i} : ' + text3) # text3에 인식한 글자가 저장됨(text3라고 한 거에는 이유없음..)