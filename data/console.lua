Console = {
	showing = false
}

function Console.Show()
	if (Console.showing) then
		return
	end

	Console.showing = true
	NativeMethods.AllocConsole()
end

function Console.Hide()
	if (not Console.showing) then
		return
	end

	Console.showing = false
	NativeMethods.FreeConsole()
end
