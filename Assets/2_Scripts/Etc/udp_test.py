import numpy as np
import json
import socket

class NumpyEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, np.ndarray):
            return obj.tolist()
        return json.JSONEncoder.default(self, obj)

class UnitySender:
    def __init__(self, ipaddr, port):
        self.ipaddr = ipaddr
        self.port = port
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM) # UDP

    def send_data(self, data):
        json_data = json.dumps(data, cls=NumpyEncoder)
        print(json_data)
        self.sock.sendto(bytes(json_data, "utf-8"), (self.ipaddr, self.port))

class JsonMaker:
    def __init__(self):
        self.data = {}

    def get_json_with_direction(self, direction):
        self.data['direction'] = direction
        return self.data

from pynput import keyboard

class KeyboardManager:
    def __init__(self):
        self.__get_json = None
        self.__send_json = None

    def run(self):
        with keyboard.Listener(
                on_press=self.on_press,
                on_release=self.on_release) as listener:
            listener.join()

        listener = keyboard.Listener(
            on_press=self.on_press,
            on_release=self.on_release)
        listener.start()

    def set_get_json_func(self, f):
        self.__get_json = f

    def set_send_json_func(self, f):
        self.__send_json = f

    def on_press(self, key):
        direction = ''
        if key == keyboard.Key.up:
            direction = 'up'
        elif key == keyboard.Key.down:
            direction = 'down'
        elif key == keyboard.Key.left:
            direction = 'left'
        elif key == keyboard.Key.right:
            direction = 'right'
        else:
            return

        if self.__get_json is not None:
            data = self.__get_json(direction)
        if self.__send_json is not None:
            self.__send_json(data)

    def on_release(self, key):
        if key == keyboard.Key.esc:
            return False

unity_sender = UnitySender('127.0.0.1', 50002)
json_maker = JsonMaker()
keyboard_manager = KeyboardManager()

keyboard_manager.set_get_json_func(json_maker.get_json_with_direction)
keyboard_manager.set_send_json_func(unity_sender.send_data)
keyboard_manager.run()
