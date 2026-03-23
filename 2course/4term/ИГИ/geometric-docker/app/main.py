import os
import sys
import json
from geometric_lib import circle, square  # импорт из репозитория

def from_env():
    r = os.getenv("RADIUS")
    if r:
        r = float(r)
        print("Area circle from RADIUS env:", circle.area(r))

def from_file(path):
    with open(path, "r", encoding="utf-8") as f:
        data = json.load(f)
    if "square_side" in data:
        a = float(data["square_side"])
        print("Square area from file:", square.area(a))

def from_stdin():
    try:
        text = sys.stdin.read().strip()
        if text:
            data = json.loads(text)
            if "circle_radius" in data:
                print("Circle perimeter from stdin:", circle.perimeter(float(data["circle_radius"])))
    except Exception:
        pass

if __name__ == "__main__":
    # порядок: env -> file -> stdin
    from_env()
    cfg_path = os.getenv("CONFIG_PATH", "/data/input.json")
    if os.path.exists(cfg_path):
        from_file(cfg_path)
    from_stdin()
