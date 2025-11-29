# Conflict Vessels - Web Interface

A Blazor WebAssembly web interface for the Conflict Vessels naval warfare game.

## Overview

This project provides a browser-based UI for Conflict Vessels, built with Blazor WebAssembly and CSS animations. The interface displays game grids with vessels and provides an interactive visual experience for gameplay.

## Features

- **Blazor WebAssembly**: Runs entirely in the browser with no server required
- **Reactive State Management**: Uses System.Reactive for automatic UI updates
- **CSS Animations**: Smooth transitions and visual effects including:
  - Staggered grid cell appearances
  - Hover effects on interactive cells
  - Pulsing vessel markers
  - Phase transition indicators
  - Explosion animations (ready for attack phase)
- **Responsive Design**: Mobile-friendly layout that adapts to different screen sizes
- **Component-Based Architecture**: Reusable Blazor components for grid rendering

## Project Structure

```
ConflictVessels.Web/
├── Components/
│   └── GridComponent.razor       # Grid visualization component
├── Pages/
│   └── Home.razor               # Home page
├── Services/
│   └── GameService.cs            # Game state management service
├── wwwroot/
│   └── css/
│       └── app.css               # Game styles and animations
└── Program.cs                    # Application entry point
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- A modern web browser (Chrome, Firefox, Safari, Edge)

### Running the Application

1. Navigate to the web interface directory:
   ```bash
   cd interfaces/ConflictVessels.Web
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open your browser to the URL displayed (typically `http://localhost:5000` or `https://localhost:5001`)

### Development Mode

For hot-reload during development:

```bash
dotnet watch run
```

The browser will automatically refresh when you make changes to the code.

## Architecture

### GameService

The `GameService` manages the game state and provides reactive updates to the UI:

- Creates and manages the current game instance
- Subscribes to game state changes via observables
- Notifies UI components when state changes occur
- Properly disposes of resources when games end

### GridComponent

A reusable Blazor component that renders a game grid:

- Displays grid cells with visual indicators for vessels
- Provides hover effects and click interactions
- Supports showing/hiding vessel positions
- Uses CSS Grid for responsive layout

### Home Page

The main landing page that provides an introduction to the game.

## Styling

The application uses a modern gradient-based color scheme:

- **Empty cells**: Blue water gradient
- **Occupied cells**: Red/orange vessel gradient
- **Setup phase**: Purple gradient indicator
- **Action phase**: Pink gradient indicator
- **Ended phase**: Cyan gradient indicator

All animations are CSS-based for optimal performance.

## Current Status

### Implemented

- ✅ Grid visualization with vessels
- ✅ Auto-generated games with random vessel placement
- ✅ Reactive state management
- ✅ CSS animations and effects
- ✅ Responsive design
- ✅ Phase indicators

### Planned

- ⏳ Attack phase interactions (targeting enemy grid)
- ⏳ Hit/miss visual feedback
- ⏳ Turn-based gameplay
- ⏳ Victory/defeat screens
- ⏳ Manual vessel placement interface
- ⏳ Game save/load functionality
- ⏳ Sound effects
- ⏳ Multiplayer support (future)

## Building for Production

To create a production build:

```bash
dotnet publish -c Release
```

The output will be in `bin/Release/net9.0/publish/wwwroot` and can be deployed to any static web hosting service.

## Technical Notes

### Performance

- The application runs entirely in the browser via WebAssembly
- No server communication required after initial load
- Efficient reactive updates using System.Reactive
- Smooth 60fps animations using CSS transforms

### Browser Compatibility

Requires a modern browser with WebAssembly support:
- Chrome/Edge 91+
- Firefox 89+
- Safari 15+

### Integration with Game Engine

The web interface directly references the `ConflictVessels.Engine` project, allowing it to:
- Use all game logic and rules
- Serialize/deserialize game state
- Subscribe to game events via observables
- Maintain consistency with other interfaces

## Contributing

When adding new features:

1. Place reusable components in `Components/`
2. Add page components in `Components/Pages/`
3. Use the `GameService` for state management
4. Add styles to `wwwroot/css/app.css`
5. Follow the reactive programming patterns established in the codebase

## License

This project is part of the Conflict Vessels game and shares the same license.
