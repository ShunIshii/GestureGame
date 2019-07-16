import serial #シリアル通信を行うため
import pickle #モデルをファイルとして使用する用
import socket #ソケット通信用

HOST = '127.0.0.1' #自分自身を指すIPアドレス
PORT = 5000 #ポート番号を指定（受信側と同じ）
client = socket.socket(socket.AF_INET, socket.SOCK_DGRAM) #ソケット通信開始

length = 20 #ジェスチャの長さ（連続で受信するデータの個数）
bullet_count = 0 #弾が連続で発射されないようにするためのカウント
before_x = 0 #ひとつ前の加速度を保存しておく用
print('Connecting...')
ser = serial.Serial('COM19',115200,timeout=None) #ポート番号19からシリアル受信

print('Input your name:')
name = input()
filename = 'models/model_' + name + '.sav' #入力した名前の学習モデルのファイル名

print('Loading...')
with open(filename, 'rb') as fp_model: #学習モデルファイルを開く
    loaded_model = pickle.load(fp_model) #モデルをロード
print('Do any gesture!')

try:
    while True:
        cnt = 0 #受信したデータのカウント用
        mx = [] #各軸ごとの配列を初期化
        my = []
        mz = []

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

            '''
            一度振って弾を発射した後に、
            戻す動作等でも同様に認識して弾を発射してしまうので、
            10回加速度を受信するまでは弾は発射されないための処理
            ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            '''
            if bullet_count != 0:
                if bullet_count == 10:
                    bullet_count = 0
                else:
                    bullet_count += 1

            if cnt != 0: #ジェスチャ中
                mx.append(x.rstrip('\r\n')) #シリアルから受け取ってたデータのゴミを取って配列へ追加
                my.append(y.rstrip('\r\n'))
                mz.append(z.rstrip('\r\n'))
                cnt += 1
                if cnt == length: #lengthまでいったら次のジェスチャ待ちへ
                    break
            elif abs(float(x) - float(before_x)) > 5 and bullet_count == 0: #急な加速度の変化を検出
                print('attack!')
                bullet_count = 1 #50行目のif文へ入るため
                client.sendto('0'.encode('utf-8'), (HOST, PORT)) #0を送信

            before_x = x #現在の加速度を次まで保存しておく

        m = mx + my + mz #三軸の加速度を横一列にする
        m2 = [] #リスト型にするため
        m2.append(m) #リストへ追加

        pre = loaded_model.predict(m2) #ジェスチャの判定
        print('This gesture is No.', pre[0][0])
        client.sendto(pre[0][0].encode('utf-8'), (HOST, PORT)) #判定されたジェスチャの番号を送信

except KeyboardInterrupt: # Ctrl-C を捕まえたら終了
    print('Close!')

ser.close() #シリアルを閉じる
client.close() #ソケット通信を切る
