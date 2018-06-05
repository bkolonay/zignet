import React, { Component } from 'react';
import UtcDate from '../common/UtcDate'
import TestStatusBadge from './TestStatusBadge'

class TestStatusLabel extends Component {

  _getText() {
    if (this.props.failingFromDate)
      return <span className='text-danger'>{'for ' + new UtcDate(this.props.failingFromDate).getTimeFromNow()}</span>
    else
      return <span className='text-success'>{'for ' + new UtcDate(this.props.passingFromDate).getTimeFromNow()}</span>
  }

  render() {
    return (
      <span>
        <TestStatusBadge failingFromDate={this.props.failingFromDate}/> {this._getText()}
      </span>
    );
  }
}

export default TestStatusLabel;
