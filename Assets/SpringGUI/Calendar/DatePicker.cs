
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
namespace SpringGUI
{
    public class DatePicker : UIBehaviour
    {
        public class DateTimeChangeEvent : UnityEvent<DateTime> { }
        DateTimeChangeEvent onDateTimeChange;
        private Text _dateText = null;
        private Calendar _calendar = null;
        private DateTime _dateTime = DateTime.Today;
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                _dateTime = value;
                refreshDateText();
            }
        }
        private bool flag = false;
        protected override void Awake()
        {
            onDateTimeChange = new DateTimeChangeEvent();
            _dateText = this.transform.Find("DateText").GetComponent<Text>();
            _calendar = this.transform.Find("Calendar").GetComponent<Calendar>();
            _calendar.onDayClick.AddListener(dateTime =>
            {
                DateTime = dateTime;
                onDateTimeChange?.Invoke(dateTime);
            });
            transform.Find("PickButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (flag)
                {
                    _calendar.Close();
                    flag = false;
                }
                else
                {
                    _calendar.Open();
                    flag = true;
                }
            
            });
            refreshDateText();
        }

        private void refreshDateText()
        {
            if (_calendar.DisplayType == E_DisplayType.Standard)
            {
                switch (_calendar.CalendarType)
                {
                    case E_CalendarType.Day:
                        _dateText.text = DateTime.ToShortDateString();
                        break;
                    case E_CalendarType.Month:
                        _dateText.text = DateTime.Year + "/" + DateTime.Month;
                        break;
                    case E_CalendarType.Year:
                        _dateText.text = DateTime.Year.ToString();
                        break;
                }
            }
            else
            {
                switch ( _calendar.CalendarType )
                {
                    case E_CalendarType.Day:
                        _dateText.text = DateTime.Year + "年" + DateTime.Month + "月" + DateTime.Day + "日";
                        break;
                    case E_CalendarType.Month:
                        _dateText.text = DateTime.Year + "年" + DateTime.Month + "月";
                        break;
                    case E_CalendarType.Year:
                        _dateText.text = DateTime.Year + "年";
                        break;
                }
            }
            _calendar.gameObject.SetActive(false);
        }
    }
}