import moment from 'moment';

class UtcDate {
  constructor(date) {
    this.moment = moment.utc(date);
  }

  toLocal() {
    this.moment = this.moment.local();
    return this;
  }

  getDateAndTimeString() {
    return this.moment.format('L LTS');
  }

  getTimeFromNow() {
    return this.moment.fromNow(true);
  }

  getTimeFromNowWithSuffix() {
    return this.moment.fromNow();
  }  

  getTimeFrom(date) {
    const localMoment = moment.utc(date);
    return this.moment.from(localMoment, true);
  }  
}

export default UtcDate;
