#!/usr/bin/python
#
# Copyright:
#   2014 Systematic Trading Group
#
# Authors
#   jonathan.shore@gmail.com
#

from pydotnet.clr.CLRApi import CLRApi


def ZDateTime(datetime, zone):
    """
    Create ZDateTime
    """
    clr = CLRApi.get()
    return clr.callstatic ("com.pydotnet.common.time.ZDateTime", "Parse", str(datetime), str(zone))


def ZTime(time):
    """
    Create ZTime
    """
    clr = CLRApi.get()
    return clr.new ("com.pydotnet.common.time.ZTime", str(time))


def ZTimeZone(zone):
    """
    Create ZTimeZone
    """
    clr = CLRApi.get()
    return clr.new ("com.pydotnet.common.time.ZTimeZone", zone)


def FDate(yearordate, month=None, day=None):
    """
    Create FDate
    """
    clr = CLRApi.get()
    if isinstance(yearordate, int):
        return clr.new ("com.pydotnet.fin.core.FDate", yearordate, month, day)
    else:
        return clr.new ("com.pydotnet.fin.core.FDate", str(yearordate))


def Calendar (name):
    """
    Get calendar of given name
    """
    clr = CLRApi.get()
    return clr.callstatic ("com.pydotnet.fin.core.Calendar", "Get", name)


def Period(period):
    """
    Create period
    """
    clr = CLRApi.get()
    if period is str:
        return clr.new ("com.pydotnet.fin.core.Period", str(period))
    else:
        return clr.new ("com.pydotnet.fin.core.Period", period)


def MktId(id):
    """
    Create mkt-id
    """
    clr = CLRApi.get()
    return clr.callstatic ("com.pydotnet.mkt.ids.MktId", "ToProperId", str(id))


def MktIdList(instruments):
    """
    Create mkt id list
    """
    clr = CLRApi.get()
    return clr.new ("com.pydotnet.mkt.ids.MktIdList", instruments)

