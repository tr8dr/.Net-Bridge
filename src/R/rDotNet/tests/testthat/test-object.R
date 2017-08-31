context ("object")

test_that ("object creation", {
    skip_if_not (is.net.installed(), "rDotNet built without .NET CLR server build")

    obj <- .cnew ("DateTime", 2017, 4, 1)
    year <- obj$Get("Year")    
    month <- obj$Get("Month")
    
    expect_equal(2017, year)
    expect_equal(4, month)

    ntime <- obj$AddMonths (2)
    month <- ntime$Get("Month")
    
    expect_equal(6, month)
})
