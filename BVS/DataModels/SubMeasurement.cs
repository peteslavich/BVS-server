using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;

namespace BVS.DataModels
{
    [Table( "SubMeasurement" )]
    public partial class SubMeasurement
    {
        [Display( Name = "Calculated Volume" )]
        public decimal? CalculatedVolume { get; set; }

        public Guid ClientGUID { get; set; }

        public int CreatedByID { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ID { get; set; }

        public bool IsVoided { get; set; }

        [JsonIgnore]
        public virtual Measurement Measurement { get; set; }

        public int MeasurementID { get; set; }

        [Display( Name = "SubMeasurement DateTime" )]
        public DateTime MeasurementOn { get; set; }

        [Display( Name = "Sensor1 LED1" )]
        public int Sensor1LED1 { get; set; }

        [Display( Name = "Sensor1 LED2" )]
        public int Sensor1LED2 { get; set; }

        [Display( Name = "Sensor1 LED3" )]
        public int Sensor1LED3 { get; set; }

        [Display( Name = "Sensor1 LED4" )]
        public int Sensor1LED4 { get; set; }

        [Display( Name = "Sensor1 LED5" )]
        public int Sensor1LED5 { get; set; }

        [Display( Name = "Sensor1 LED6" )]
        public int Sensor1LED6 { get; set; }

        [Display( Name = "Sensor1 LED7" )]
        public int Sensor1LED7 { get; set; }

        [Display( Name = "Sensor1 LED8" )]
        public int Sensor1LED8 { get; set; }

        [Display( Name = "Sensor2 LED1" )]
        public int Sensor2LED1 { get; set; }

        [Display( Name = "Sensor2 LED2" )]
        public int Sensor2LED2 { get; set; }

        [Display( Name = "Sensor2 LED3" )]
        public int Sensor2LED3 { get; set; }

        [Display( Name = "Sensor2 LED4" )]
        public int Sensor2LED4 { get; set; }

        [Display( Name = "Sensor2 LED5" )]
        public int Sensor2LED5 { get; set; }

        [Display( Name = "Sensor2 LED6" )]
        public int Sensor2LED6 { get; set; }

        [Display( Name = "Sensor2 LED7" )]
        public int Sensor2LED7 { get; set; }

        [Display( Name = "Sensor2 LED8" )]
        public int Sensor2LED8 { get; set; }

        [Display( Name = "Sensor3 LED1" )]
        public int Sensor3LED1 { get; set; }

        [Display( Name = "Sensor3 LED2" )]
        public int Sensor3LED2 { get; set; }

        [Display( Name = "Sensor3 LED3" )]
        public int Sensor3LED3 { get; set; }

        [Display( Name = "Sensor3 LED4" )]
        public int Sensor3LED4 { get; set; }

        [Display( Name = "Sensor3 LED5" )]
        public int Sensor3LED5 { get; set; }

        [Display( Name = "Sensor3 LED6" )]
        public int Sensor3LED6 { get; set; }

        [Display( Name = "Sensor3 LED7" )]
        public int Sensor3LED7 { get; set; }

        [Display( Name = "Sensor3 LED8" )]
        public int Sensor3LED8 { get; set; }

        [Display( Name = "Sensor4 LED1" )]
        public int Sensor4LED1 { get; set; }

        [Display( Name = "Sensor4 LED2" )]
        public int Sensor4LED2 { get; set; }

        [Display( Name = "Sensor4 LED3" )]
        public int Sensor4LED3 { get; set; }

        [Display( Name = "Sensor4 LED4" )]
        public int Sensor4LED4 { get; set; }

        [Display( Name = "Sensor4 LED5" )]
        public int Sensor4LED5 { get; set; }

        [Display( Name = "Sensor4 LED6" )]
        public int Sensor4LED6 { get; set; }

        [Display( Name = "Sensor4 LED7" )]
        public int Sensor4LED7 { get; set; }

        [Display( Name = "Sensor4 LED8" )]
        public int Sensor4LED8 { get; set; }

        [Display( Name = "Sensor5 LED1" )]
        public int Sensor5LED1 { get; set; }

        [Display( Name = "Sensor5 LED2" )]
        public int Sensor5LED2 { get; set; }

        [Display( Name = "Sensor5 LED3" )]
        public int Sensor5LED3 { get; set; }

        [Display( Name = "Sensor5 LED4" )]
        public int Sensor5LED4 { get; set; }

        [Display( Name = "Sensor5 LED5" )]
        public int Sensor5LED5 { get; set; }

        [Display( Name = "Sensor5 LED6" )]
        public int Sensor5LED6 { get; set; }

        [Display( Name = "Sensor5 LED7" )]
        public int Sensor5LED7 { get; set; }

        [Display( Name = "Sensor5 LED8" )]
        public int Sensor5LED8 { get; set; }

        [Display( Name = "Sensor6 LED1" )]
        public int Sensor6LED1 { get; set; }

        [Display( Name = "Sensor6 LED2" )]
        public int Sensor6LED2 { get; set; }

        [Display( Name = "Sensor6 LED3" )]
        public int Sensor6LED3 { get; set; }

        [Display( Name = "Sensor6 LED4" )]
        public int Sensor6LED4 { get; set; }

        [Display( Name = "Sensor6 LED5" )]
        public int Sensor6LED5 { get; set; }

        [Display( Name = "Sensor6 LED6" )]
        public int Sensor6LED6 { get; set; }

        [Display( Name = "Sensor6 LED7" )]
        public int Sensor6LED7 { get; set; }

        [Display( Name = "Sensor6 LED8" )]
        public int Sensor6LED8 { get; set; }

        [Display( Name = "Sensor7 LED1" )]
        public int Sensor7LED1 { get; set; }

        [Display( Name = "Sensor7 LED2" )]
        public int Sensor7LED2 { get; set; }

        [Display( Name = "Sensor7 LED3" )]
        public int Sensor7LED3 { get; set; }

        [Display( Name = "Sensor7 LED4" )]
        public int Sensor7LED4 { get; set; }

        [Display( Name = "Sensor7 LED5" )]
        public int Sensor7LED5 { get; set; }

        [Display( Name = "Sensor7 LED6" )]
        public int Sensor7LED6 { get; set; }

        [Display( Name = "Sensor7 LED7" )]
        public int Sensor7LED7 { get; set; }

        [Display( Name = "Sensor7 LED8" )]
        public int Sensor7LED8 { get; set; }

        [Display( Name = "Sensor8 LED1" )]
        public int Sensor8LED1 { get; set; }

        [Display( Name = "Sensor8 LED2" )]
        public int Sensor8LED2 { get; set; }

        [Display( Name = "Sensor8 LED3" )]
        public int Sensor8LED3 { get; set; }

        [Display( Name = "Sensor8 LED4" )]
        public int Sensor8LED4 { get; set; }

        [Display( Name = "Sensor8 LED5" )]
        public int Sensor8LED5 { get; set; }

        [Display( Name = "Sensor8 LED6" )]
        public int Sensor8LED6 { get; set; }

        [Display( Name = "Sensor8 LED7" )]
        public int Sensor8LED7 { get; set; }

        [Display( Name = "Sensor8 LED8" )]
        public int Sensor8LED8 { get; set; }

        public int UpdatedByID { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}