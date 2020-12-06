Namespace Core

    Public Class AmongCustomFuncs


        Public Shared Function GetColorFromID(ByVal ColorID As Integer) As Color
            Select Case ColorID
                Case 0 : Return Color.FromArgb(198, 17, 17) 'Rojo
                Case 1 : Return Color.FromArgb(19, 46, 210) 'Azul
                Case 2 : Return Color.FromArgb(17, 128, 45) 'Verde
                Case 3 : Return Color.FromArgb(238, 84, 187) 'Rosa
                Case 4 : Return Color.FromArgb(240, 125, 13) 'Naranja
                Case 5 : Return Color.FromArgb(246, 246, 87) 'Amarillo
                Case 6 : Return Color.FromArgb(63, 71, 78) 'Negro
                Case 7 : Return Color.FromArgb(215, 225, 241) 'Blanco
                Case 8 : Return Color.FromArgb(107, 47, 188) 'Morado
                Case 9 : Return Color.FromArgb(113, 73, 30) 'Marron
                Case 10 : Return Color.FromArgb(56, 255, 221) 'Celeste
                Case 11 : Return Color.FromArgb(80, 240, 57) 'Lima
            End Select
            Return Color.White
        End Function

        'Eine mathematische Funktion, um einen Radian in Degree umzuwandeln
        Public Shared Function radtodeg(ByVal radian As Double) As Double
            Return (radian / 180) * Math.PI
        End Function

        Public Shared Function WorldToRadar(ByVal deinePosition As Point, ByVal gegnerPosition As Point, ByVal deinYaw As Single, ByVal radarBreite As Single, ByVal radarHöhe As Single, ByVal Radius As Integer) As Point
            Try


                'Berechnet Kosinus und Sinus von Yaw
                Dim cosYaw As Single = Math.Cos(radtodeg(deinYaw))
                Dim sinYaw As Single = Math.Sin(radtodeg(deinYaw))



                'Wir berechnen die Distanz zwischen uns und dem anderen Mitspieler
                Dim distanzX As Single = gegnerPosition.X - deinePosition.X
                Dim distanzY As Single = gegnerPosition.Y - deinePosition.Y

                'Wir berechnen die Radarkoordinaten
                Dim positionX As Single = ((distanzY * cosYaw - distanzX * sinYaw) * -1 / Radius) + radarBreite / 2
                Dim positionY As Single = ((distanzX * cosYaw + distanzY * sinYaw) / Radius) * -1 + radarHöhe / 2



                'Wir passen die Radarkoordinaten an unseren Spieler (in der Mitte) an
                Return New Point(positionX, positionY)

            Catch ex As Exception
            End Try
        End Function

    End Class

End Namespace

