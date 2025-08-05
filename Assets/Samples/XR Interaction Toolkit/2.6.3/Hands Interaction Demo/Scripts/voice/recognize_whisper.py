import whisper
import sounddevice as sd
import numpy as np
import scipy.io.wavfile
import sys
import difflib

EXPECTED = "我們一起看看家裡有哪些東西需要補充,這樣我們才不會買到重複的東西,你看這些蔬菜已經夠了,我們來買些其他的吧!"

def record_audio(filename='input.wav', duration=10, fs=16000):
    print("開始錄音...")
    audio = sd.rec(int(duration * fs), samplerate=fs, channels=1)
    sd.wait()
    scipy.io.wavfile.write(filename, fs, audio)
    print("錄音完成！")

def transcribe(filename='input.wav'):
    model = whisper.load_model("base")  # 可換成 tiny/medium
    result = model.transcribe(filename, language='zh')
    return result['text']

def compare_text(target, spoken):
    ratio = difflib.SequenceMatcher(None, target, spoken).ratio()
    return ratio > 0.8  # 相似度門檻可調整

if __name__ == "__main__":
    record_audio()
    text = transcribe()
    print("辨識結果：", text)
    correct = compare_text(EXPECTED, text)
    if correct:
        print("CORRECT")
        sys.exit(0)
    else:
        print("INCORRECT")
        sys.exit(1)
