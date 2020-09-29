using System;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;

using TollFeeCalculator;

/// <summary>
///
/// </summary>
public class TollCalculator
{

    /// <summary>
    /// Calculate the total toll fee for one day
    /// </summary>
    /// <param name="vehicle">the vehicle</param>
    /// <param name="dates">date and time of all passes on one day</param>
    /// <returns>the total toll fee for that day</returns>
    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(vehicle, date);
            int tempFee = GetTollFee(vehicle, intervalStart);

            if ((date - intervalStart).TotalMinutes() <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        if (date.isTimeBetween("06:00", "06:29")) return 8;
        if (date.isTimeBetween("06:30", "06:59")) return 13;
        if (date.isTimeBetween("07:00", "07:59")) return 18;
        if (date.isTimeBetween("08:00", "08:29")) return 13;
        if (date.isTimeBetween("08:30", "14:59")) return 8;
        if (date.isTimeBetween("15:00", "15:29")) return 13;
        if (date.isTimeBetween("15:30", "16:59")) return 18;
        if (date.isTimeBetween("17:00", "17:59")) return 13;
        if (date.isTimeBetween("18:00", "18:29")) return 8;
        return 0;
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        //if (IsRedDay(date)) return true;
        //return false;
        return IsRedDay(date);
    }

    /// <summary>
    /// Calulate the date if Easter of an Year
    /// </summary>
    /// <param name="Year_of_easter"></param>
    /// <returns>EasterSunday</returns>
    public DateTime EasterSunday(int Year_of_easter)
    {
        int y = Year_of_easter;
        int d = (((255 - 11 * (y % 19)) - 21) % 30) + 21;
        DateTime easter_date = new DateTime(y, 3, 1);
        easter_date = easter_date.AddDays(+d + (d > 48) + 6 - ((y + y / 4 + d + (d > 48) + 1) % 7));
        return (easter_date);
    }

    /// <summary>
    /// Datum 2020	Namn						Beräkning
    ///  1 jan		Nyårsdagen[Not 1]			Fast datum, 1 januari
    ///  5 jan		Trettondagsafton[Not 2]		Fast datum, 5 januari
    ///  6 jan		Trettondedag jul			Fast datum, 6 januari(Trettondedagen)
    ///  9 apr		Skärtorsdagen[Not 2]		Rörligt datum, torsdagen före Påskdagen
    /// 10 apr		Långfredagen				Rörligt datum, fredagen före Påskdagen
    /// 11 apr		Påskafton					Rörligt datum, lördagen före Påskdagen.
    /// 12 apr		Påskdagen					Rörligt datum, första söndagen efter ecklesiastisk fullmåne, efter vårdagjämningen
    /// 13 apr		Annandag påsk				Rörligt datum, dagen efter påskdagen
    /// 30 apr		Valborgsmässoafton[Not 2]	Fast datum, 30 april
    ///  1 maj		Första maj					Fast datum, 1 maj
    /// 21 maj		Kristi himmelsfärdsdag		Rörligt datum, sjätte torsdagen efter påskdagen
    /// 30 maj		Pingstafton					Rörligt datum, dagen före pingstdagen
    /// 31 maj		Pingstdagen					Rörligt datum, sjunde söndagen efter påskdagen
    ///  6 jun		Sveriges nationaldag		Fast datum, 6 juni
    /// 19 jun		Midsommarafton[Not 3]		Rörligt datum, fredagen mellan 19 juni och 25 juni (fredagen före midsommardagen)
    /// 20 jun		Midsommardagen				Rörligt datum, lördagen mellan 20 juni och 26 juni
    /// 30 okt		Allhelgonaafton[Not 2]		Rörligt datum, fredag mellan 30 oktober och 5 november
    /// 31 okt		Alla helgons dag			Rörligt datum, lördagen som infaller under perioden från 31 oktober till 6 november
    /// 24 dec		Julafton[Not 3]				Fast datum, 24 december
    /// 25 dec		Juldagen					Fast datum, 25 december
    /// 26 dec		Annandag jul				Fast datum, 26 december
    /// 31 dec		Nyårsafton[Not 3]			Fast datum, 31 december
    /// Noter:
    ///  1. Föregående dag, alltså 31 december nederst i tabellen, räknas som söndag enligt 3 a § i Semesterlagen och är därmed arbetsfri.
    ///  2. [a b c d] Denna dag har inte lagstadgad ställning som arbetsfri dag.Många arbetsplatser har ändå förkortad arbetstid enligt kollektivavtal eller lokala avtal.
    ///  3. [a b c] Denna dag räknas som söndag enligt 3 a § i Semesterlagen och är därmed arbetsfri.
    /// </summary>
    /// <param name="d">Date to test for Redday</param>
    /// <returns>True is Swedish Red Day</returns>
    private Boolean IsRedDay(DateTime d)
    {
        // July, Saturday and Sunday are TollFree
        if (d.Month == 7) return true;
        if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday) return true;

        int y = d.Year;
        DateTime Easter = EasterSunday(y);
        Boolean res = false;
        switch (d)
        {
            case DateTime(y, 1, 1): res = true; break;                                  //	Nyårsdagen[Not 1]
        //  case DateTime(y, 1, 5): res = true; break;                                  //	Trettondagsafton[Not 2]
            case DateTime(y, 1, 6): res = true; break;                                  //	Trettondedag jul
        //  case Easter.AddDays(-3): res = true; break;                                 //	Skärtorsdagen[Not 2]
            case Easter.AddDays(-2): res = true; break;                                 //	Långfredagen
        //  case Easter.AddDays(-1): res = true; break;                                 //	Påskafton
            case Easter: res = true; break;                                             //	Påskdagen
            case Easter.AddDays(1): res = true; break;                                  //	Annandag påsk
            case DateTime(y, 4, 30): res = true; break;                                 //	Valborgsmässoafton[Not 2]
            case DateTime(y, 5, 1): res = true; break;                                  //	Första maj
            case Easter.AddDays(39): res = true; break;                                 //	Kristi himmelsfärdsdag
            case Easter.AddDays(48): res = true; break;                                 //	Pingstafton
            case Easter.AddDays(49): res = true; break;                                 //	Pingstdagen
        //  case Easter.AddDays(50): res = true; break;                                 //	Annandag Pingst[slopad]
            case DateTime(y, 5, 6): res = true; break;                                  //	Sveriges nationaldag
            case DateTime(y, 6, 18).GetNextDay(DayOfWeek.Friday) : res = true; break;   //	Midsommarafton[Not 3]
            case DateTime(y, 6, 19).GetNextDay(DayOfWeek.Saturday) : res = true; break; //	Midsommardagen
                                                                                        //  July is Tollfree
        //  case DateTime(y, 10, 29).GetNextDay(DayOfWeek.Friday): res = true; break;   //	Allhelgonaafton[Not 2]
            case DateTime(y, 10, 30).GetNextDay(DayOfWeek.Saturday) res = true; break;  //	Alla helgons dag
            case DateTime(y, 12, 24): res = true; break;                                //	Julafton[Not 3]
            case DateTime(y, 12, 25): res = true; break;                                //	Juldagen
            case DateTime(y, 12, 26): res = true; break;                                //	Annandag jul
            case DateTime(y, 12, 31): res = true; break;                                //	Nyårsafton[Not 3]
            default: break;
        }
    }

    private enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}