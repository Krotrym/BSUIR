#!/usr/bin/env python3
import argparse
import csv
from pathlib import Path


COLORS = [
    "#1f77b4",
    "#d62728",
    "#2ca02c",
    "#9467bd",
    "#ff7f0e",
    "#17becf",
]


def read_points(path):
    points = []
    with open(path, newline="", encoding="utf-8") as f:
        reader = csv.DictReader(f)
        for row in reader:
            try:
                x = float(row["elapsed_seconds"])
                y = float(row["iterations"])
            except (KeyError, TypeError, ValueError):
                continue
            points.append((x, y))
    if not points:
        raise ValueError(f"{path}: no elapsed_seconds/iterations data")
    return points


def nice_label(path):
    return Path(path).stem.replace("_", " ")


def make_svg(series, title, width=1100, height=650):
    margin_left = 90
    margin_right = 30
    margin_top = 70
    margin_bottom = 80
    plot_w = width - margin_left - margin_right
    plot_h = height - margin_top - margin_bottom

    xs = [x for _, points in series for x, _ in points]
    ys = [y for _, points in series for _, y in points]
    x_min, x_max = min(xs), max(xs)
    y_min, y_max = 0.0, max(ys)
    if x_max == x_min:
        x_max = x_min + 1.0
    if y_max == y_min:
        y_max = y_min + 1.0

    def sx(x):
        return margin_left + (x - x_min) / (x_max - x_min) * plot_w

    def sy(y):
        return margin_top + plot_h - (y - y_min) / (y_max - y_min) * plot_h

    lines = []
    lines.append(f'<svg xmlns="http://www.w3.org/2000/svg" width="{width}" height="{height}" viewBox="0 0 {width} {height}">')
    lines.append('<rect width="100%" height="100%" fill="white"/>')
    lines.append(f'<text x="{width / 2}" y="34" text-anchor="middle" font-family="Arial" font-size="24" font-weight="700">{title}</text>')

    for i in range(6):
        gx = margin_left + i * plot_w / 5
        gy = margin_top + i * plot_h / 5
        x_val = x_min + i * (x_max - x_min) / 5
        y_val = y_max - i * (y_max - y_min) / 5
        lines.append(f'<line x1="{gx:.2f}" y1="{margin_top}" x2="{gx:.2f}" y2="{margin_top + plot_h}" stroke="#e5e7eb"/>')
        lines.append(f'<line x1="{margin_left}" y1="{gy:.2f}" x2="{margin_left + plot_w}" y2="{gy:.2f}" stroke="#e5e7eb"/>')
        lines.append(f'<text x="{gx:.2f}" y="{margin_top + plot_h + 28}" text-anchor="middle" font-family="Arial" font-size="13" fill="#374151">{x_val:.2f}</text>')
        lines.append(f'<text x="{margin_left - 12}" y="{gy + 4:.2f}" text-anchor="end" font-family="Arial" font-size="13" fill="#374151">{y_val:.0f}</text>')

    lines.append(f'<line x1="{margin_left}" y1="{margin_top}" x2="{margin_left}" y2="{margin_top + plot_h}" stroke="#111827" stroke-width="2"/>')
    lines.append(f'<line x1="{margin_left}" y1="{margin_top + plot_h}" x2="{margin_left + plot_w}" y2="{margin_top + plot_h}" stroke="#111827" stroke-width="2"/>')
    lines.append(f'<text x="{width / 2}" y="{height - 25}" text-anchor="middle" font-family="Arial" font-size="16">Время выполнения, с</text>')
    lines.append(f'<text x="24" y="{height / 2}" text-anchor="middle" font-family="Arial" font-size="16" transform="rotate(-90 24 {height / 2})">Количество итераций</text>')

    for idx, (label, points) in enumerate(series):
        color = COLORS[idx % len(COLORS)]
        polyline = " ".join(f"{sx(x):.2f},{sy(y):.2f}" for x, y in points)
        lines.append(f'<polyline points="{polyline}" fill="none" stroke="{color}" stroke-width="3"/>')
        for x, y in points[::max(1, len(points) // 25)]:
            lines.append(f'<circle cx="{sx(x):.2f}" cy="{sy(y):.2f}" r="3" fill="{color}"/>')
        legend_x = margin_left + 20
        legend_y = margin_top + 24 + idx * 24
        lines.append(f'<rect x="{legend_x}" y="{legend_y - 12}" width="18" height="4" fill="{color}"/>')
        lines.append(f'<text x="{legend_x + 28}" y="{legend_y - 6}" font-family="Arial" font-size="14" fill="#111827">{label}</text>')

    lines.append("</svg>")
    return "\n".join(lines)


def main():
    parser = argparse.ArgumentParser(description="Build SVG graph from lab CSV files.")
    parser.add_argument("csv_files", nargs="+", help="CSV files with elapsed_seconds and iterations columns")
    parser.add_argument("-o", "--out", default="graph.svg", help="Output SVG path")
    parser.add_argument("--title", default="График выполнения программы", help="Graph title")
    args = parser.parse_args()

    series = [(nice_label(path), read_points(path)) for path in args.csv_files]
    svg = make_svg(series, args.title)
    Path(args.out).write_text(svg, encoding="utf-8")
    print(f"written {args.out}")


if __name__ == "__main__":
    main()
