# Aim-Lab-Light-Demo

This is light version of aim lab to practice aim for games

## Screenshots

![StartScreen](/ScreenshotsAndVideos/screenshot1.png?raw=true "Screen 1")
![GameScreen](/ScreenshotsAndVideos/screenshot2.png?raw=true "Screen 2")
![WinScreen](/ScreenshotsAndVideos/screenshot3.png?raw=true "Screen 3")

# Features

## Target System

- Always maintains 5 active targets on screen

- Targets spawn at random non-overlapping positions

- Uses Object Pooling for performance

- Targets react instantly to hits

- Missed shots tracked without target lifetime timer

## Shooting Mechanics

- Center-screen reticle aiming

- Adjustable mouse sensitivity

- Raycast-based shooting with UI click detection exclusion

- Miss detection when firing without hitting a target

## Scoring & Stats

- +10 points per hit

- –5 points per miss

Tracks:

- Hits

- Misses

- Average reaction speed

- Final score (score × reaction-speed multiplier)

- Saves best average reaction speed in PlayerPrefs

## Session Timer

- 2-minute session system

- Pausable via ESC

- Automatically calculates final score when session ends

## Scene Management

- Simple, reusable scene controller

- Menu → Game → Results navigation

- Supports quit & reload
