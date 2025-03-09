import can

bus = can.Bus(channel="PCAN_USBBUS1", interface="pcan")  # ✅ `bustype="pcan"` 사용

print("⏳ CAN 메시지 수신 대기 중...")
recv_msg = bus.recv(timeout=30)  # ✅ 30초 대기

if recv_msg:
    print(f"✅ 수신된 CAN 메시지: ID={recv_msg.arbitration_id}, Data={recv_msg.data}")
else:
    print("⚠ 메시지 수신 타임아웃")

bus.shutdown()
