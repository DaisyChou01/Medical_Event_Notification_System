# Feature: Medecal Event Notification System

## 1. Data Source

### Tables

* Opds

  * OpdDate (string, format: YYYYMMDD)
  * IsFirstTime (Y/N)

* Inpts

  * YearMonth (string, YYYYMM)
  * Visits (int)
  * BedDays (int)
  * OccRate (decimal)

* Ers

  * YearMonth (string)
  * TriageVisits (int)
  * BoardingVisits (int)
  * AdmittedVisits (int)

* Yms

  * Yearmonth (string)

---

## 2. Time Definition

* Current Month (YM): max(Yms.Yearmonth)
* Last Year Month (lastYM): YM - 100

---

## 3. KPI Definitions

### OPD

* currentTotal = count(Opds where OpdDate starts with YM)

* lastYearTotal = count(Opds where OpdDate starts with lastYM)

* totalGrowthRate = (currentTotal - lastYearTotal) / lastYearTotal * 100

* currentFirst = count(Opds where YM and IsFirstTime = Y)

* firstGrowthRate = same logic

* firstVisitRate = currentFirst / currentTotal * 100

---

### Inpatient

* currentInTotal = sum(Visits where YM)

* InTotalGrowthRate = same logic

* currentBeddays = sum(BedDays)

* BeddaysGrowthRate

* ALOS = BedDays / Visits

* OccRate = avg(OccRate)

---

### ER

* currentERTotal = sum(TriageVisits)

* ERTotalGrowthRate

* BoardingGrowthRate

* AdmissionRate = AdmittedVisits / TriageVisits

---

## 4. Rules (Abnormal Conditions)

Trigger alert when ANY condition is met:

* totalGrowthRate < -10
* firstGrowthRate < -10
* rateGrowthRate < -10
* InTotalGrowthRate < -10
* BeddaysGrowthRate < -10
* AlosGrowthRate > 10
* OccrateGrowthRate < -10
* ERTotalGrowthRate < -10
* BordingsGrowthRate > 10
* AdmissionRateGrowthRate < -10

---

## 5. Notification

### Trigger

* When any rule is triggered

### Channel

* Email

### Content

* Table format
* Only include abnormal KPIs

---

## 6. System Behavior

* On page load:

  1. Calculate KPI
  2. Evaluate rules
  3. If abnormal → send email

---

## 7. Constraints

* Do NOT redesign database
* Do NOT simplify formulas
* Must match original business logic exactly

---

## 8. Non-Functional Requirements

* Use async/await
* Separate service layers
* Avoid loading full tables into memory
