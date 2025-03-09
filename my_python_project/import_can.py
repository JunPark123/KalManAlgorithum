import can

try:
    # 가상 CAN 버스 연결 테스트
    bus = can.Bus("test_virtual_can", interface="virtual")  # ✅ 최신 python-can 대응
    print("✅ 가상 CAN 버스가 정상적으로 생성되었습니다.")
except Exception as e:
    print("⚠ 가상 CAN 버스를 생성할 수 없습니다. 에러 메시지:", e)
