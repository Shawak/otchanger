function getLuaFunctions()-- by Mock
	local str = ""
	for f,k in pairs(_G) do
		if type(k) == 'function' then
			str = str..f..','
		elseif type(k) == 'table' then
			for d,o in pairs(k) do
				if type(o) == 'function' then
					if f ~= '_G' and d ~= "_G" and f ~= 'package' then
						str = str..f.."."..d..','
					end
				elseif type(o) == 'table' then
					for m,n in pairs(o) do
						if type(n) == 'function' then
							if d == "_M" and m ~= "_M" and f ~= "_G" and f ~= 'package' then
								str = str..f.."."..m..","
							elseif f ~= '_G' and m ~= "_G" and d ~= "_G" and f ~= 'package' then
								str = str..f.."."..d..'.'..m..','
							end
						elseif type(n) == 'table' then
							for x,p in pairs(n) do
								if type(p) == 'function' then
									if m == "_M" and d ~= "_M" and f ~= "_G" and f ~= 'package' then
										str = str..f.."."..d..'.'..x..','
									elseif m == "_M" and d == "_M" and f ~= "_G" and f ~= 'package' then
										str = str..f.."."..x..','
									elseif m ~= "_M" and d == "_M" and f ~= "_G" and f ~= 'package' then
										str = str..f..'.'..m..'.'..x..','
									elseif f ~= '_G' and m ~= "_G" and d ~= "_G" and f ~= 'package' then
										str = str..f.."."..d..'.'..m..'.'..x..','
									end
								end
							end
						end
					end
				end
			end
		end
	end
	return string.explode(str,',')
end

local k = getLuaFunctions()
--- Create file content your server function list
local file__ = io.open('luafunctions.txt','w')
table.sort(k)
for i=1,#k do
    if k[i] ~= "" then
        file__:write((i-1)..' - '..k[i]..'\n')
    end
end
file__:close()