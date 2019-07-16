#coding utf-8
import serial #シリアル通信を行うため
import numpy as np #numpyで2次元配列を扱うため
from sklearn.svm import SVC #学習用
import pickle #モデルをファイルとして使用する用

length = 20 #ジェスチャの長さ（連続で受信するデータの個数）
gestures = 3 #ジェスチャの種類の数
times = 3 #各ジェスチャを行う回数

print('Connecting...')
ser = serial.Serial('COM19',115200,timeout=None) #ポート番号19からシリアル受信

print('Input your name:')
name = input()
filename = 'models/model_' + name + '.sav' #入力した名前の学習モデルのファイル名
X = []
Y = []

for i in range(gestures * times):
    cnt = 0 #受信したデータのカウント用
    mx = [] #各軸ごとの配列を初期化
    my = []
    mz = []
    gesture_num = i % gestures + 1 #ジェスチャ番号を指定

    print('Do gesture No.', gesture_num)

    while True:
        line = ser.readline() #シリアルから1行取得
        switch = line.decode('utf-8') #コード変換
        line = ser.readline()
        x = line.decode('utf-8')
        line = ser.readline()
        y = line.decode('utf-8')
        line = ser.readline()
        z = line.decode('utf-8')

        if switch == 'clicked\r\n' and cnt == 0: #クリックされたらカウント開始
            cnt = 1

        if cnt != 0: #ジェスチャ中
            mx.append(x.rstrip('\r\n')) #シリアルから受け取ってたデータのゴミを取って配列へ追加
            my.append(y.rstrip('\r\n'))
            mz.append(z.rstrip('\r\n'))
            cnt += 1
            if cnt == length: #lengthまでいったら次のジェスチャ待ちへ
                break

    m = mx + my + mz #三軸の加速度を横一列にする
    X.append(m) #学習データ用リストへ追加
    Y.append(str(gesture_num)) #学習ラベル用リストへ追加

ser.close() #シリアルを閉じる

model = SVC(kernel = 'linear', C=1, gamma=1) #学習モデルのパラメータを指定
model.fit(X,np.ravel(Y)) #学習を行う

with open(filename, 'wb') as fp_model: #学習モデルを保存するためのファイルを開く
    pickle.dump(model, fp_model) #モデルを保存

print('Model created.')
