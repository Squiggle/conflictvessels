# Rules

- There exists two identically-sized boards
  - Each board is owned by a player
  - A board is square or rectangular grid divided up into spaces
  - Each space can be described in an X/Y coordinate where 'X' is numeric and
    'Y' is one or more characters
  - Each space can contain either part of a Vessel or no Vessel at all
- There exists a number of Players
  - Each Player owns a Board
- A Game is comprised of a number of Players and a sequence of Phases
  - The first Phase of the Game comprises of each Player populating their Board
    with a selection of Vessels
    - The Player is presented with a selection of Vessels
    - The Player populates their board by selecting the position and rotation of
      each Vessel in turn so that each Vessel is:
      - Contained wholly within the board
      - Does not overlap on any previously placed Vessel
  - The second Phase of the Game is the Attack phase
    - This Phase is a sequence of Actions, taken in turn by each Player
    - Each Action is comprised of:
      1. Nominating one Square on another Player's board to Attack. You cannot
         nominate a Square that has already been Attacked during this Game
      2. Being informed whether that Attack connected with a Vessel or not (a
         Hit or a Miss)
      3. Being informed whether that Attack was the final Hit on a Vessel (a
         sunken ship), and the attacking player being informed what type of
         Vessel was struck.
    - The Phase ends as soon as one Player's board contains only Vessels which
      have been revealed as Sunk. That player loses, and the other player wins.
