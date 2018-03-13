import React, { Component } from 'react';
import UtcDate from '../common/UtcDate'

class TestStatusLabel extends Component {

  _getBadge() {
    if (this.props.failingFromDate)
      return <span className='badge badge-danger'>Down</span>
    else
      return <span className='badge badge-success'>Up</span>
  }

  _getText() {
    if (this.props.failingFromDate)
      return <span className='text-danger'>{'for ' + new UtcDate(this.props.failingFromDate).getTimeFromNow()}</span>
    else
      return <span className='text-success'>{'for ' + new UtcDate(this.props.passingFromDate).getTimeFromNow()}</span>
  }

  render() {
    return (
      <span>
        {this._getBadge()} {this._getText()}
      </span>
    );
  }
}

export default TestStatusLabel;
