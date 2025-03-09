import can
import threading
import time

bus = can.Bus(channel="PCAN_USBBUS1", interface="virtual")  # ✅ `bustype="pcan"` 사용

def receive_can():
    print("⏳ CAN 메시지 수신 대기 중...")
    recv_msg = bus.recv(timeout=10)
    if recv_msg:
        print(f"✅ 수신된 CAN 메시지: ID={recv_msg.arbitration_id}, Data={recv_msg.data}")
    else:
        print("⚠ 메시지 수신 타임아웃")

def send_can():
    time.sleep(1)  # 수신이 시작될 때까지 기다림
    msg = can.Message(arbitration_id=0x123, data=[0x11, 0x22, 0x33, 0x44], is_extended_id=False)
    bus.send(msg)
    print(f"✅ 송신된 CAN 메시지: ID={msg.arbitration_id}, Data={msg.data}")

recv_thread = threading.Thread(target=receive_can)
send_thread = threading.Thread(target=send_can)

recv_thread.start()
send_thread.start()

recv_thread.join()
send_thread.join()

bus.shutdown()