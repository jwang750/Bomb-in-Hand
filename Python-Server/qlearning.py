# -*- coding: utf-8 -*-
"""
Created on Sat Apr 10 12:45:47 2021

@author: yijingxiao
"""

import numpy as np
import pandas as pd
import random
from pyServer import PyServer

class Game:
    rewards = None
    positionCol = None
    positionRow = None
    destinationCol = None
    destinationRow = None
    playerCol = None
    
    #GAME INITIALIZE VARIABLE NEEDED FROM UNITY: startCol, startRow, playerCol and hole_location
    def __init__(self, startCol=1, startRow=1, destinationRow = 5,destinationCol = 4,  playerCol = 1, hole_location = []):
        self.rewards = np.array([[0,0,0,0], 
                                 [0,0,0,0], 
                                 [0,0,0,0],
                                 [0,0,0,0],
                                 [0,0,0,0]])
        self.startCol = startCol
        self.startRow = startRow
        
        self.positionCol = startCol
        self.positionRow = startRow
        self.playerCol = playerCol
        
        if hole_location!=[]:
            self.rewards[hole_location[0]-1][hole_location[1]-1]=-1000

        for i in range(0, len(self.rewards)): #row iterator
            for j in range(0, len(self.rewards[0])): #column iterater
                if self.rewards[i][j]!=-1000: #if it is not a hole
                    self.rewards[i][j] = abs(i-(len(self.rewards)//2))*20+abs(j-(self.playerCol-1))*100-(len(self.rewards)-i)*40
        
        result = np.where(self.rewards == np.amax(self.rewards))
        
        self.destinationRow = result[0][0]+1
        self.destinationCol = result[1][0]+1

    def move(self, direction):
        reward = 0
        end = False
#         print(direction)
        if direction=='Up':
            if self.positionRow==1:
                return (-1000, True)
            else:
                self.positionRow -= 1    
        elif direction=='Down':
            if self.positionRow == len(self.rewards):
#                 print("here")
                pass
            else:
                self.positionRow += 1
        elif direction=='Left':
            if self.positionCol == 1:
                pass
            else: 
                self.positionCol -= 1  
        else:
            if self.positionCol == len(self.rewards[0]):
                pass
            else: 
                self.positionCol += 1
        
        
        if self.positionCol == self.destinationCol and self.positionRow == self.destinationRow:
            end = True
            reward = self.rewards[self.positionRow-1][self.positionCol-1]
#             print(self.positionCol)
#             print(self.positionRow)
        else:
#             print(self.positionCol)
#             print(self.positionRow)
#             print(len(self.rewards))
            end = False
            reward = self.rewards[self.positionRow-1][self.positionCol-1]
        return (reward, end)
        
    def info(self):
        print("Game Initialized, Reward Table: ")
        print(self.rewards)
        print("Destination Row: ", self.destinationRow)
        print("Destination Column: ", self.destinationCol)
        
def onReceive(startCol,startRow,playerCol,hole_location):
    game = Game(startCol=startCol, startRow=startRow, destinationRow=5, destinationCol=4, playerCol=playerCol, hole_location=hole_location)
    game.info()
    # states are in columns and actions are in rows
    learning_rate = 1
    discount = 0
    random_explore = 0.1
    qtable = pd.DataFrame(100, index=['Up', 'Down', 'Left', 'Right'],
                          columns=[11, 12, 13, 14,
                                   21, 22, 23, 24,
                                   31, 32, 33, 34,
                                   41, 42, 43, 44,
                                   51, 52, 53, 54])

    for i in range(1000):
        # print ("Game # " + str(i))
        game = Game(startCol=startCol, startRow=startRow, destinationRow=5, destinationCol=4, playerCol=playerCol, hole_location=hole_location)
        end_of_game = False
        while not end_of_game:
            # get current state
            current_state = (game.positionRow * 10) + game.positionCol
            # select the action with maximum reward
            max_reward_action = qtable[current_state].idxmax()
            # replace with random action to promote exploration and not get stuck in a loop
            if random.random() < random_explore:
                max_reward_action = qtable.index[random.randint(0, 3)]
            # play the game with that action
            reward, end_of_game = game.move(max_reward_action)

            if end_of_game:
                qtable.loc[max_reward_action, current_state] = reward

            else:
                opimtal_future_value = qtable[(game.positionRow * 10) + game.positionCol].max()
                discounted_opimtal_future_value = discount * opimtal_future_value
                learned_value = reward + discounted_opimtal_future_value
                qtable.loc[max_reward_action, current_state] = \
                    (1 - learning_rate) * qtable[current_state][max_reward_action] + learning_rate * learned_value

    move_list = ["UP", "DOWN", "LEFT", "RIGHT"]
    move_steps = []
    for i in range(len(qtable.values[0])):
        index = np.where(qtable.values[:, i] == np.amax(qtable.values[:, i]))[0][0]
        move = move_list[index]
        move_steps.append(move)

    cur_Row = game.startRow
    cur_Col = game.startCol
    move_steps_df = pd.DataFrame(np.array(move_steps).reshape((1, 20)),
                                 columns=[11, 12, 13, 14,
                                          21, 22, 23, 24,
                                          31, 32, 33, 34,
                                          41, 42, 43, 44,
                                          51, 52, 53, 54])

    final_moving_array = np.array([["", "", "", ""],
                                   ["", "", "", ""],
                                   ["", "", "", ""],
                                   ["", "", "", ""],
                                   ["", "", "", ""]], dtype='>U2')
    for column in move_steps_df.columns:
        row = column // 10 - 1
        col = column - (row + 1) * 10 - 1

        if move_steps_df[column][0] == "UP":
            final_moving_array[row][col] = " ^"
        elif move_steps_df[column][0] == "DOWN":
            final_moving_array[row][col] = " v"
        elif move_steps_df[column][0] == "LEFT":
            final_moving_array[row][col] = " <"
        elif move_steps_df[column][0] == "RIGHT":
            final_moving_array[row][col] = " >"

    #print(final_moving_array)

    final_moving_steps = []
    position = cur_Row * 10 + cur_Col
    final_position = game.destinationRow * 10 + game.destinationCol
    while position != final_position:
        old = final_moving_array[cur_Row - 1][cur_Col - 1]
        #     print("*"+old)
        final_moving_array[cur_Row - 1][cur_Col - 1] = "*" + old[1:]
        if move_steps_df[position][0] == "UP":
            cur_Row -= 1
        elif move_steps_df[position][0] == "DOWN":
            cur_Row += 1
        elif move_steps_df[position][0] == "LEFT":
            cur_Col -= 1
        elif move_steps_df[position][0] == "RIGHT":
            cur_Col += 1
        final_moving_steps.append(move_steps_df[position][0])
        position = cur_Row * 10 + cur_Col

    #print(final_moving_steps)  # THIS IS THE LIST TO BE PASSED TO UNITY
    #print(final_moving_array)
    #print(qtable)
    return final_moving_steps

server = PyServer(PORT = 8888)
# initial location is fixed
startCol = 1
startRow = 1
playerCol = 1
hole_location = []
final_moving_steps = onReceive(startCol,startRow,playerCol,hole_location)
#python send msg to unity
server.update_m(final_moving_steps)
while True:
    res = server.waiting()
    print("receive from unity:",res)
    # res is a list of 4 elements from unity
    # res[0] = playerCol; res[1] = startCol ; res[2] = startRow; res[3] = holeLoc
    playerCol = int(res[0])
    startCol = int(res[1])
    startRow = int(res[2])
    if(int(res[3])!=-1): #if holeLoc = -1, then there is no new hole
        hole_location = [int(res[3])]
    else:
        hole_location = []
    final_moving_steps = onReceive(startCol, startRow, playerCol, hole_location)
    server.update_m(final_moving_steps)
    print("send message to unity!")