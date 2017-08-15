#!/usr/bin/python
#
# Copyright:
#   2014 Systematic Trading Group
#
# Authors
#   jonathan.shore@gmail.com
#

import pandas as pd
import numpy as np

from datetime import datetime
from pydotnet.clr.CLRApi import CLRApi


def _toDataFrame (self):
    """
    Convert RTimeSeries back to data frame
    """
    ## get data matrix & times
    data = self.Data
    stamps = self.Times

    ## transform and assign times as index
    times = [datetime.utcfromtimestamp(t) for t in stamps]
    data.index = times
    return data


class RTimeSeries1D:
    """
    Create C#-side timeseries, can be constructed either with:
        - a CLR-side timeseries OR
        - instrument & data-frame
    """

    @staticmethod
    def fromFrame (instrument, series: pd.DataFrame):
        """
        :param instrument: instrument name
        :param series: data frame containing time series
        """
        clr = CLRApi.get()

        times = (series.index.values - np.datetime64('1970-01-01T00:00:00Z')) / np.timedelta64(1, 's')
        data = pd.DataFrame (series.values, columns=series.columns)

        rbars = clr.new ("RTimeSeries1D", instrument, times, data)
        rbars.addMethod ("toDataFrame", _toDataFrame)
        return rbars


    @staticmethod
    def fromObj (series):
        """
        :param series: CLR-side TimeSeries<T> object
        """
        clr = CLRApi.get()

        rbars = clr.new ("RTimeSeries1D", series)
        rbars.addMethod ("toDataFrame", _toDataFrame)
        return rbars


class TimeSeriesMatrix:
    """
    Create C#-side TimeSeriesMatrix
    """

    @staticmethod
    def fromFrame (series: pd.DataFrame, period: str, zone: str):
        """
        :param series: data frame containing time series
        """
        clr = CLRApi.get()

        times = (series.index.values - np.datetime64('1970-01-01T00:00:00Z')) / np.timedelta64(1, 's')
        data = pd.DataFrame (series.values, columns=series.columns)

        cperiod = clr.new ("com.pydotnet.fin.core.Period", period)
        czone = clr.new ("com.pydotnet.common.time.ZTimeZone", zone)

        rbars = clr.new ("TimeSeriesMatrix", times, data, cperiod, czone)
        rbars.addMethod ("toDataFrame", _toDataFrame)
        return rbars


    @staticmethod
    def fromFilename (filename: str, period: str, zone: str, stride: int):
        """
        :param series: data frame containing time series
        """
        clr = CLRApi.get()

        cperiod = clr.new ("com.pydotnet.fin.core.Period", period)
        czone = clr.new ("com.pydotnet.common.time.ZTimeZone", zone)

        rbars = clr.new ("TimeSeriesMatrix", filename, cperiod, czone, stride, None)
        return rbars
