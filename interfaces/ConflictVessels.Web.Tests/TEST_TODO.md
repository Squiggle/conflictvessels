# ConflictVessels.Web - Testing TODO List

This document tracks all components and services that require unit tests to meet the 95% code coverage requirement.

## Testing Priority: CRITICAL

**Status**: ConflictVessels.Web currently has NO TESTS. All items below must be implemented before adding new features.

---

## Services

### GameService (`Services/GameService.cs`)

**Priority**: HIGH - Core service managing game state

Test file: `Services/GameServiceTests.cs`

#### Test Cases Required:

- [ ] **Constructor and initialization**
  - [ ] `Constructor_initializes_with_null_game`
  - [ ] `Constructor_initializes_empty_subscriptions_list`
  - [ ] `CurrentGame_returns_null_initially`

- [ ] **StartNewGame method**
  - [ ] `StartNewGame_creates_new_game_instance`
  - [ ] `StartNewGame_creates_game_with_two_players`
  - [ ] `StartNewGame_creates_arena_with_two_grids`
  - [ ] `StartNewGame_creates_grids_with_correct_dimensions`
  - [ ] `StartNewGame_creates_grids_with_five_vessels_each`
  - [ ] `StartNewGame_creates_vessels_with_correct_sizes`
  - [ ] `StartNewGame_invokes_OnGameStateChanged_event`
  - [ ] `StartNewGame_disposes_previous_game_when_called_multiple_times`
  - [ ] `StartNewGame_clears_previous_subscriptions`
  - [ ] `StartNewGame_subscribes_to_phase_changes`

- [ ] **GetPlayerGrid method**
  - [ ] `GetPlayerGrid_returns_null_when_game_is_null`
  - [ ] `GetPlayerGrid_returns_null_for_negative_index`
  - [ ] `GetPlayerGrid_returns_null_for_index_exceeding_grid_count`
  - [ ] `GetPlayerGrid_returns_correct_grid_for_player_0`
  - [ ] `GetPlayerGrid_returns_correct_grid_for_player_1`
  - [ ] `GetPlayerGrid_returns_different_grids_for_different_players`

- [ ] **OnGameStateChanged event**
  - [ ] `OnGameStateChanged_fires_when_game_phase_changes`
  - [ ] `OnGameStateChanged_fires_when_StartNewGame_called`
  - [ ] `OnGameStateChanged_does_not_fire_after_unsubscribe`

- [ ] **Dispose method**
  - [ ] `Dispose_clears_all_subscriptions`
  - [ ] `Dispose_disposes_current_game`
  - [ ] `Dispose_sets_game_to_null`
  - [ ] `Dispose_can_be_called_multiple_times_safely`
  - [ ] `Dispose_unsubscribes_from_phase_observable`

**Estimated test count**: 25+ tests

---

## Components

### GridComponent (`Components/GridComponent.razor`)

**Priority**: HIGH - Core UI component for displaying game grids

Test file: `Components/GridComponentTests.cs`

#### Test Cases Required:

- [ ] **Rendering**
  - [ ] `Component_renders_without_errors`
  - [ ] `Component_throws_when_Grid_parameter_is_null`
  - [ ] `Component_renders_correct_number_of_cells`
  - [ ] `Component_renders_cells_with_correct_dimensions`
  - [ ] `Component_applies_grid_size_css_variable`
  - [ ] `Component_renders_empty_grid_when_no_vessels`

- [ ] **Grid parameter**
  - [ ] `Component_renders_10x10_grid_for_default_grid`
  - [ ] `Component_updates_when_Grid_parameter_changes`
  - [ ] `Component_renders_different_sized_grids_correctly`

- [ ] **ShowVessels parameter**
  - [ ] `ShowVessels_true_displays_vessel_markers`
  - [ ] `ShowVessels_false_hides_vessel_markers`
  - [ ] `ShowVessels_defaults_to_true`
  - [ ] `ShowVessels_only_shows_markers_on_occupied_cells`

- [ ] **Cell rendering**
  - [ ] `Cells_have_empty_class_when_no_vessel`
  - [ ] `Cells_have_occupied_class_when_vessel_present`
  - [ ] `Cells_have_correct_title_attribute_with_coordinates`
  - [ ] `Cell_coordinates_use_letter_number_notation`
  - [ ] `Vessel_marker_rendered_only_on_vessel_cells`

- [ ] **GetVesselAtCoords method**
  - [ ] `GetVesselAtCoords_returns_null_for_empty_cell`
  - [ ] `GetVesselAtCoords_returns_vessel_when_present`
  - [ ] `GetVesselAtCoords_returns_correct_vessel_for_multiple_vessels`
  - [ ] `GetVesselAtCoords_handles_edge_coordinates`

- [ ] **GetCellClass method**
  - [ ] `GetCellClass_returns_empty_for_null_vessel`
  - [ ] `GetCellClass_returns_occupied_for_vessel`

- [ ] **OnCellClicked event callback**
  - [ ] `OnCellClick_invokes_callback_with_correct_coordinates`
  - [ ] `OnCellClick_does_not_throw_when_callback_not_set`
  - [ ] `OnCellClick_passes_coords_with_correct_x_value`
  - [ ] `OnCellClick_passes_coords_with_correct_y_value`
  - [ ] `OnCellClick_works_for_all_grid_positions`

- [ ] **User interactions**
  - [ ] `Clicking_cell_triggers_OnCellClicked_event`
  - [ ] `Multiple_cell_clicks_trigger_multiple_events`
  - [ ] `Clicked_cell_coordinates_match_visual_position`

**Estimated test count**: 30+ tests

---

### Home Page (`Pages/Home.razor`)

**Priority**: MEDIUM - Simple landing page

Test file: `Pages/HomeTests.cs`

#### Test Cases Required:

- [ ] **Rendering**
  - [ ] `Component_renders_without_errors`
  - [ ] `Component_displays_Conflict_Vessels_heading`
  - [ ] `Component_displays_game_description`

- [ ] **Page metadata**
  - [ ] `PageTitle_is_set_to_Conflict_Vessels`
  - [ ] `Page_route_is_root_path`

**Estimated test count**: 5 tests

---

### NavMenu (`Layout/NavMenu.razor`)

**Priority**: MEDIUM - Navigation component

Test file: `Layout/NavMenuTests.cs`

#### Test Cases Required:

- [ ] **Rendering**
  - [ ] `Component_renders_without_errors`
  - [ ] `Component_displays_navbar_brand`
  - [ ] `Navbar_brand_text_is_ConflictVessels_Web`
  - [ ] `Component_has_navbar_toggler_button`

- [ ] **Navigation links**
  - [ ] `Component_renders_Home_nav_link`
  - [ ] `Component_renders_Play_Game_nav_link`
  - [ ] `Home_link_points_to_root_route`
  - [ ] `Play_Game_link_points_to_game_route`
  - [ ] `Home_link_has_correct_icon_class`
  - [ ] `Play_Game_link_has_correct_icon_class`

- [ ] **Navigation menu toggle**
  - [ ] `NavMenu_is_collapsed_by_default`
  - [ ] `ToggleNavMenu_expands_collapsed_menu`
  - [ ] `ToggleNavMenu_collapses_expanded_menu`
  - [ ] `Clicking_toggler_button_toggles_menu`
  - [ ] `Clicking_nav_menu_area_toggles_menu`
  - [ ] `NavMenuCssClass_returns_collapse_when_collapsed`
  - [ ] `NavMenuCssClass_returns_null_when_expanded`

- [ ] **CSS classes**
  - [ ] `Component_applies_correct_navbar_classes`
  - [ ] `Navigation_area_applies_nav_scrollable_class`

**Estimated test count**: 18 tests

---

## Additional Infrastructure

### Test Utilities (if needed)

Test file: `TestUtilities/` (create as needed)

- [ ] Mock GameService implementations
- [ ] Test grid factories
- [ ] Common test data builders

---

## Summary

| Component/Service | Priority | Estimated Tests | Status |
|-------------------|----------|----------------|--------|
| GameService | HIGH | 25+ | ❌ Not Started |
| GridComponent | HIGH | 30+ | ❌ Not Started |
| Game Page | HIGH | 30+ | ❌ Not Started |
| Home Page | MEDIUM | 8 | ❌ Not Started |
| NavMenu | MEDIUM | 18 | ❌ Not Started |

**Total Estimated Tests**: ~110+ tests

**Current Coverage**: 0%
**Target Coverage**: ≥95%

---

## Notes

1. All tests must follow AAA pattern (Arrange-Act-Assert)
2. Use descriptive test names with underscores
3. Tests should be independent and isolated
4. Mock external dependencies where appropriate
5. Use bUnit's TestContext for component testing
6. Verify coverage with `dotnet test /p:CollectCoverage=true`

---

**Next Steps**:
1. Start with HIGH priority tests (GameService, GridComponent, Game Page)
2. Implement tests incrementally
3. Run coverage after each component is tested
4. Aim for 95%+ coverage on each component before moving to next
