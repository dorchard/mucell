x = [[1],[2],[3]]
main = do print $ foldl (\x y -> x++y) [] x