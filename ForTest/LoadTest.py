import subprocess
import time

def start_notepad():
    subprocess.Popen(['notepad.exe'])

def stop_notepad():
    subprocess.run(['taskkill', '/f', '/im', 'notepad.exe'])

if __name__ == "__main__":
    try:
        while True:
            start_notepad()
            time.sleep(0.5)
            stop_notepad()
            time.sleep(0.5)
    except KeyboardInterrupt:
        print("Test stopped by user")
