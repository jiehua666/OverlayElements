Add-Type -AssemblyName System.Drawing

$CARD_W = 768
$CARD_H = 960
$outputDir = "C:\Users\PC\Desktop\项目文件夹\03_美术资源\card_frames"
if (-not (Test-Path $outputDir)) { New-Item -ItemType Directory -Path $outputDir -Force | Out-Null }

function Create-Frame {
    param($name, $P, $S, $A, $G)
    
    $bmp = New-Object System.Drawing.Bitmap($CARD_W, $CARD_H, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::Transparent)
    
    $borderW = 16
    $innerMargin = 12
    $midY = $CARD_H / 2
    
    # Main background
    $brushP = New-Object System.Drawing.SolidBrush($P)
    $g.FillEllipse($brushP, 4, 4, $CARD_W-8, $CARD_H-8)
    
    # Border
    $penA = New-Object System.Drawing.Pen($A, 3)
    $g.DrawEllipse($penA, $borderW, $borderW, $CARD_W-2*$borderW, $CARD_H-2*$borderW)
    
    # Upper section
    $alpha1 = [System.Drawing.Color]::FromArgb(180, $P.R, $P.G, $P.B)
    $brushA1 = New-Object System.Drawing.SolidBrush($alpha1)
    $g.FillRectangle($brushA1, $innerMargin+$borderW, $innerMargin+$borderW, $CARD_W-2*($innerMargin+$borderW), $midY-$innerMargin-$borderW-20)
    
    # Lower section
    $alpha2 = [System.Drawing.Color]::FromArgb(200, $S.R, $S.G, $S.B)
    $brushA2 = New-Object System.Drawing.SolidBrush($alpha2)
    $g.FillRectangle($brushA2, $innerMargin+$borderW, $midY+20, $CARD_W-2*($innerMargin+$borderW), $CARD_H-$midY-$innerMargin-$borderW-20)
    
    # Divider line
    $g.DrawLine($penA, $innerMargin+$borderW+20, $midY, $CARD_W-$innerMargin-$borderW-20, $midY)
    
    # Diamond
    $ds = 14
    $brushA = New-Object System.Drawing.SolidBrush($A)
    $pt1 = New-Object System.Drawing.Point([int]($CARD_W/2), [int]($midY-$ds))
    $pt2 = New-Object System.Drawing.Point([int]($CARD_W/2+$ds), [int]$midY)
    $pt3 = New-Object System.Drawing.Point([int]($CARD_W/2), [int]($midY+$ds))
    $pt4 = New-Object System.Drawing.Point([int]($CARD_W/2-$ds), [int]$midY)
    $pts = [System.Drawing.Point[]]@($pt1,$pt2,$pt3,$pt4)
    $g.FillPolygon($brushA, $pts)
    
    # Corner ornaments
    $brushG = New-Object System.Drawing.SolidBrush($G)
    $cx1 = $innerMargin+$borderW+20; $cy1 = $innerMargin+$borderW+20
    $cx2 = $CARD_W-$innerMargin-$borderW-20; $cy2 = $innerMargin+$borderW+20
    $cx3 = $innerMargin+$borderW+20; $cy3 = $CARD_H-$innerMargin-$borderW-20
    $cx4 = $CARD_W-$innerMargin-$borderW-20; $cy4 = $CARD_H-$innerMargin-$borderW-20
    
    $g.FillEllipse($brushG, $cx1-25, $cy1-25, 50, 50)
    $g.FillEllipse($brushA, $cx1-12, $cy1-12, 24, 24)
    $g.FillEllipse($brushG, $cx2-25, $cy2-25, 50, 50)
    $g.FillEllipse($brushA, $cx2-12, $cy2-12, 24, 24)
    $g.FillEllipse($brushG, $cx3-25, $cy3-25, 50, 50)
    $g.FillEllipse($brushA, $cx3-12, $cy3-12, 24, 24)
    $g.FillEllipse($brushG, $cx4-25, $cy4-25, 50, 50)
    $g.FillEllipse($brushA, $cx4-12, $cy4-12, 24, 24)
    
    # Side decorations
    for ($i = 0; $i -lt 3; $i++) {
        $yOff = $midY - 90 + $i * 55
        $g.FillRectangle($brushA, $innerMargin+$borderW+10, $yOff, 5, 40)
        $g.FillRectangle($brushA, $CARD_W-$innerMargin-$borderW-15, $yOff, 5, 40)
    }
    
    # Text
    $font = New-Object System.Drawing.Font("Arial", 22, [System.Drawing.FontStyle]::Bold)
    $fontLg = New-Object System.Drawing.Font("Arial", 30, [System.Drawing.FontStyle]::Bold)
    $whiteBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::White)
    $sf = New-Object System.Drawing.StringFormat
    $sf.Alignment = [System.Drawing.StringAlignment]::Center
    $sf.LineAlignment = [System.Drawing.StringAlignment]::Center
    
    $g.DrawString("[ PORTRAIT ]", $font, $brushA, ($CARD_W/2), 75, $sf)
    $g.DrawString("[ EFFECT ]", $font, $brushA, ($CARD_W/2), ($midY+70), $sf)
    $g.DrawString("[ CARD NAME ]", $fontLg, $whiteBrush, ($CARD_W/2), ($midY+150), $sf)
    
    $g.Dispose()
    $filename = "$outputDir\card_frame_$name.png"
    $bmp.Save($filename, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    Write-Host "OK: $filename"
}

# Fire
Create-Frame "fire" ([System.Drawing.Color]::FromArgb(220,60,30)) ([System.Drawing.Color]::FromArgb(255,140,0)) ([System.Drawing.Color]::FromArgb(255,200,50)) ([System.Drawing.Color]::FromArgb(255,100,0))

# Water
Create-Frame "water" ([System.Drawing.Color]::FromArgb(30,100,200)) ([System.Drawing.Color]::FromArgb(80,160,255)) ([System.Drawing.Color]::FromArgb(150,200,255)) ([System.Drawing.Color]::FromArgb(100,180,255))

# Wind
Create-Frame "wind" ([System.Drawing.Color]::FromArgb(180,200,220)) ([System.Drawing.Color]::FromArgb(220,230,240)) ([System.Drawing.Color]::FromArgb(255,255,255)) ([System.Drawing.Color]::FromArgb(200,220,255))

# Wood
Create-Frame "wood" ([System.Drawing.Color]::FromArgb(60,120,50)) ([System.Drawing.Color]::FromArgb(100,160,80)) ([System.Drawing.Color]::FromArgb(180,220,120)) ([System.Drawing.Color]::FromArgb(150,200,100))

Write-Host "Done!"
