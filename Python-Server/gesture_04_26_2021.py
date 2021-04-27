# -*- coding: utf-8 -*-
"""
Created on Tue Mar  2 11:19:56 2021

@author: yijingxiao
"""

import cv2
import mediapipe as mp
from pyServer import PyServer

# reference: https://gist.github.com/TheJLifeX/74958cc59db477a91837244ff598ef4a

### Functions
def recognizeHandGesture(landmarks):
  thumbState = 'UNKNOWN'
  indexFingerState = 'UNKNOWN'
  middleFingerState = 'UNKNOWN'
  ringFingerState = 'UNKNOWN'
  littleFingerState = 'UNKNOWN'
  recognizedHandGesture = None

  pseudoFixKeyPoint = landmarks[2]['x']
  if (landmarks[3]['x'] < pseudoFixKeyPoint and landmarks[4]['x'] < landmarks[3]['x']):
    thumbState = 'CLOSE'    
  elif (pseudoFixKeyPoint < landmarks[3]['x'] and landmarks[3]['x'] < landmarks[4]['x']):
    thumbState = 'OPEN'    

  pseudoFixKeyPoint = landmarks[6]['y']
  if (landmarks[7]['y'] < pseudoFixKeyPoint and landmarks[8]['y'] < landmarks[7]['y']):
    indexFingerState = 'OPEN'    
  elif (pseudoFixKeyPoint < landmarks[7]['y'] and landmarks[7]['y'] < landmarks[8]['y']):
    indexFingerState = 'CLOSE'    

  pseudoFixKeyPoint = landmarks[10]['y']
  if (landmarks[11]['y'] < pseudoFixKeyPoint and landmarks[12]['y'] < landmarks[11]['y']):
    middleFingerState = 'OPEN'    
  elif (pseudoFixKeyPoint < landmarks[11]['y'] and landmarks[11]['y'] < landmarks[12]['y']):
    middleFingerState = 'CLOSE'

  pseudoFixKeyPoint = landmarks[14]['y']
  if (landmarks[15]['y'] < pseudoFixKeyPoint and landmarks[16]['y'] < landmarks[15]['y']):
    ringFingerState = 'OPEN'    
  elif (pseudoFixKeyPoint < landmarks[15]['y'] and landmarks[15]['y'] < landmarks[16]['y']):
    ringFingerState = 'CLOSE'
  
  pseudoFixKeyPoint = landmarks[18]['y']
  if (landmarks[19]['y'] < pseudoFixKeyPoint and landmarks[20]['y'] < landmarks[19]['y']):
    littleFingerState = 'OPEN'    
  elif (pseudoFixKeyPoint < landmarks[19]['y'] and landmarks[19]['y'] < landmarks[20]['y']):
    littleFingerState = 'CLOSE'
    
  if (thumbState == 'OPEN' and indexFingerState == 'OPEN' and middleFingerState == 'OPEN' and ringFingerState == 'OPEN' and littleFingerState == 'OPEN'):
    recognizedHandGesture = 5 # "FIVE"   
  elif (thumbState == 'CLOSE' and indexFingerState == 'OPEN' and middleFingerState == 'OPEN' and ringFingerState == 'OPEN' and littleFingerState == 'OPEN'):
    recognizedHandGesture = 4 # "FOUR"  
  elif (thumbState == 'OPEN' and indexFingerState == 'OPEN' and middleFingerState == 'OPEN' and ringFingerState == 'CLOSE' and littleFingerState == 'CLOSE'):
    recognizedHandGesture = 3 # "TREE"   
  elif (thumbState == 'OPEN' and indexFingerState == 'OPEN' and middleFingerState == 'CLOSE' and ringFingerState == 'CLOSE' and littleFingerState == 'CLOSE'):
    recognizedHandGesture = 2 # "TWO"   
  elif (thumbState == 'CLOSE' and indexFingerState == 'CLOSE' and middleFingerState == 'CLOSE' and ringFingerState == 'CLOSE' and littleFingerState == 'CLOSE'):
    recognizedHandGesture = 10 # "FIST"
  else:
    recognizedHandGesture = 0 # "UNKNOWN"
  return recognizedHandGesture

def getStructuredLandmarks(landmarks):
  structuredLandmarks = []
  for j in range(42):
    if( j %2 == 1):
      structuredLandmarks.append({ 'x': landmarks[j - 1], 'y': landmarks[j] })
  return structuredLandmarks

if __name__ == '__main__':
  server = PyServer(7777)
  mp_drawing = mp.solutions.drawing_utils
  mp_hands = mp.solutions.hands

  font = cv2.FONT_HERSHEY_SIMPLEX
  bottomLeftCornerOfText = (10, 300)
  fontScale = 1
  fontColor = (255, 255, 255)
  lineType = 2

  # For webcam input:
  cap = cv2.VideoCapture(0)
  hands = mp_hands.Hands(min_detection_confidence=0.5, min_tracking_confidence=0.5)
  while cap.isOpened():
    success, image = cap.read()
    if not success:
      print("Ignoring empty camera frame.")
      # If loading a video, use 'break' instead of 'continue'.
      continue

    # Flip the image horizontally for a later selfie-view display, and convert
    # the BGR image to RGB.
    image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)
    # To improve performance, optionally mark the image as not writeable to
    # pass by reference.
    image.flags.writeable = False
    results = hands.process(image)

    # Draw the hand annotations on the image.
    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
    if results.multi_hand_landmarks:
      for hand_landmarks in results.multi_hand_landmarks:
        mp_drawing.draw_landmarks(image, hand_landmarks, mp_hands.HAND_CONNECTIONS)
        land_mark_str = str(hand_landmarks).strip().split("\n")
        landmark_data = []
        for i in range(21):
          x = float(land_mark_str[i * 5 + 1].strip().split(":")[1])
          y = float(land_mark_str[i * 5 + 2].strip().split(":")[1])
          landmark_data.append(x)
          landmark_data.append(y)
        recognizedHandGesture = recognizeHandGesture(getStructuredLandmarks(landmark_data))
        if recognizedHandGesture==0:
            print("recognized hand gesture: ", recognizedHandGesture)
        else:
            print("recognized hand gesture: ", 1)
    else:
      recognizedHandGesture = -1
    if recognizedHandGesture == -1:
      cv2.putText(image, "No Hands Detected", bottomLeftCornerOfText, font, fontScale, fontColor, lineType)
      server.update_n()
    elif recognizedHandGesture == 0:
      cv2.putText(image, "Fist, Adding Power", bottomLeftCornerOfText, font, fontScale, fontColor, lineType)
      server.update_k()
    else:
      cv2.putText(image, "Release Bomb", bottomLeftCornerOfText, font, fontScale, fontColor, lineType)
      server.update_l()
    cv2.imshow('MediaPipe Hands', image)
    if cv2.waitKey(5) & 0xFF == 27:
      break
  cap.release()

