import numpy as np

class KalmanFilter:
    def __init__(self, initial_state, initial_uncertainty, process_noise, measurement_noise):
        self.x = np.array([[initial_state]])
        self.P = np.array([[initial_uncertainty]])
        self.Q = np.array([[process_noise]])
        self.R = np.array([[measurement_noise]])
        self.F = np.array([[1]])
        self.H = np.array([[1]])

    def predict(self):
        self.x = np.dot(self.F, self.x)
        self.P = np.dot(np.dot(self.F, self.P), self.F.T) + self.Q

    def update(self, measurement):
        K = np.dot(self.P, self.H.T) / (np.dot(self.H, np.dot(self.P, self.H.T)) + self.R)
        self.x = self.x + K * (measurement - np.dot(self.H, self.x))
        self.P = self.P - np.dot(K, np.dot(self.H, self.P))
        return self.x[0, 0]

# C#에서 호출할 함수
def run_kalman_filter(measurements):
    kf = KalmanFilter(initial_state=0, initial_uncertainty=1, process_noise=0.1, measurement_noise=1)
    results = []
    for z in measurements:
        kf.predict()
        results.append(kf.update(z))
    return results
