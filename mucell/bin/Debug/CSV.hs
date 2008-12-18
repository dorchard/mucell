module CSV (parseCSV) where

import Text.ParserCombinators.Parsec

csvFile = endBy line eol
line = sepBy cell (char ',')
cell = many (noneOf ",\n")
eol = char '\n'

parseCSV :: String -> IO (Either ParseError [[String]])
parseCSV filename = do parseFromFile csvFile filename 